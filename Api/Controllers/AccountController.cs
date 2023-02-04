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

    public AccountController(IAuthManager authManager)
    {
        _authManager = authManager;
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult> Register([FromBody] ApiUserDto apiUserDto)
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

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        AuthResponseDto? authResponse = await _authManager.Login(loginDto);

        if (authResponse is null)
        {
            return Unauthorized();
        }

        return Ok(authResponse);
    }

    [HttpPost]
    public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] AuthResponseDto authResponseDto)
    {
        AuthResponseDto authResponse = await _authManager.VerifyRefreshToken(authResponseDto);

        return Ok(authResponse);
    }
}