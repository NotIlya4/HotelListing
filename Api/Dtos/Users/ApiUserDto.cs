﻿namespace Api.Dtos.Users;

public class ApiUserDto : LoginDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}