namespace Api.Dtos.Country;

public abstract class BaseCountryDto
{
    public required string Name { get; set; }
    public required string ShortName { get; set; }
}