using Api.Dtos.Users;
using Microsoft.AspNetCore.Identity;

namespace Api.Interfaces;

public interface IAuthManager
{
    Task<IEnumerable<IdentityError>> RegisterUser(ApiUserDto userDto);
    Task<AuthResponseDto?> Login(LoginDto loginDto);
    Task<string> CreateRefreshToken();
    Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request);
}