namespace Api.Dtos.Users;

public class AuthResponseDto
{
    public required string UserId { get; set; }
    public required string Token { get; set; }
    public string? RefreshToken { get; set; }
}