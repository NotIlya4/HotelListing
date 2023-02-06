using Api.Dtos.Users;
using Api.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAuthManager _authManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IAuthManager authManager, ILogger<AccountController> logger)
    {
        _authManager = authManager;
        _logger = logger;
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult> Register([FromBody] ApiUserDto apiUserDto)
    {
        _logger.LogInformation($"Registration attempt for {apiUserDto.Email}");

        try
        {
            List<IdentityError> errors = new(await _authManager.RegisterUser(apiUserDto));

            if (errors.Count > 0)
            {
                foreach (IdentityError error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Something went wrong in the {nameof(Register)}");
            return Problem($"Something went wrong in the {nameof(Register)}", statusCode: 500);
        }
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        _logger.LogInformation($"Login Attempt for {loginDto.Email} ");
        try
        {
            AuthResponseDto? authResponse = await _authManager.Login(loginDto);

            if (authResponse is null)
            {
                return Unauthorized();
            }

            return Ok(authResponse);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Something went wrong in the {nameof(Login)}");
            return Problem($"Something went wrong in the {nameof(Login)}", statusCode: 500);
        }
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] AuthResponseDto authResponseDto)
    {
        AuthResponseDto authResponse = await _authManager.VerifyRefreshToken(authResponseDto);

        return Ok(authResponse);
    }
}