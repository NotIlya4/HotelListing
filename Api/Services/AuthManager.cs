using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Dtos.Users;
using Api.Entities;
using Api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services;

public class AuthManager : IAuthManager
{
    private readonly IMapper _mapper;
    private readonly UserManager<ApiUser> _userManager;
    private readonly IConfiguration _configuration;
    private ApiUser? _user;
    
    private const string LoginProvider = "HotelListingApi";
    private const string RefreshToken = "RefreshToken";

    public AuthManager(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration)
    {
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
    }
    
    public async Task<IEnumerable<IdentityError>> RegisterUser(ApiUserDto userDto)
    {
        _user = _mapper.Map<ApiUser>(userDto);
        _user.UserName = userDto.Email;

        IdentityResult result = await _userManager.CreateAsync(_user, userDto.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(_user, "User");
        }

        return result.Errors;
    }

    public async Task<AuthResponseDto?> Login(LoginDto loginDto)
    {
        _user = await _userManager.FindByEmailAsync(loginDto.Email);
        
        if (_user is null)
        {
            return null;
        }

        bool isValidUser = await _userManager.CheckPasswordAsync(_user, loginDto.Password);

        if (isValidUser == false)
        {
            return null;
        }
        
        string token = await GenerateToken();
        return new AuthResponseDto
        {
            Token = token,
            UserId = _user.Id
        };
    }

    public async Task<string> CreateRefreshToken()
    {
        if (_user is null)
        {
            throw new NullReferenceException();
        }
        await _userManager.RemoveAuthenticationTokenAsync(_user, LoginProvider, RefreshToken);
        var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user, LoginProvider, RefreshToken);
        return newRefreshToken;
    }

    public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request)
    {
        if (request.RefreshToken is null)
        {
            throw new NullReferenceException();
        }
        
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
        JwtSecurityToken tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token) ?? throw new NullReferenceException();
        string? username = tokenContent.Claims.FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Email)?.Value;
        if (username is null)
        {
            throw new NullReferenceException();
        } 
        _user = await _userManager.FindByNameAsync(username);

        if (_user is null)
        {
            throw new NullReferenceException();
        }

        bool isValidRefreshToken =
            await _userManager.VerifyUserTokenAsync(_user, LoginProvider, RefreshToken, request.RefreshToken);

        if (isValidRefreshToken == false)
        {
            throw new NullReferenceException();
        }

        var token = await GenerateToken();
        return new AuthResponseDto
        {
            UserId = _user.Id,
            Token = token,
            RefreshToken = await CreateRefreshToken()
        };
    }

    private async Task<string> GenerateToken()
    {
        string? securityKeyString = _configuration["JwtSettings:Key"];
        
        if (securityKeyString is null)
        {
            throw new NullReferenceException();
        } 
        
        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(securityKeyString));
        
        SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        if (_user is null)
        {
            throw new NullReferenceException();
        }
        
        IList<string> roles = await _userManager.GetRolesAsync(_user);

        IList<Claim> roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();

        IList<Claim> userClaims = await _userManager.GetClaimsAsync(_user);

        List<Claim> claims = new()
        {
            new Claim(JwtRegisteredClaimNames.Sub, _user.Email ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, _user.Email ?? ""),
            new Claim("uid", _user.Id)
        };
        claims = claims.Union(userClaims).Union(roleClaims).ToList();

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(int.Parse(_configuration["JwtSettings:DurationInMinutes"] ?? throw new NullReferenceException())),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}