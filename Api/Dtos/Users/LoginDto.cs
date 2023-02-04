using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Users;

public class LoginDto
{
    [EmailAddress]
    public required string Email { get; set; }
    [StringLength(15, ErrorMessage = "Your Password is limited to {2} to {1} characters", MinimumLength = 6)]
    public required string Password { get; set; }
}