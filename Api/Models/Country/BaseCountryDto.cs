namespace Api.Models.Country;

public abstract class BaseCountryDto
{
    public required string Name { get; set; }
    public required string ShortName { get; set; }
}