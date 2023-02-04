using Microsoft.AspNetCore.Identity;

namespace Api.Entities;

public class ApiUser : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}