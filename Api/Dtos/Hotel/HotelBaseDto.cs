namespace Api.Dtos.Hotel;

public abstract class HotelBaseDto
{
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required double Rating { get; set; }
    public required int CountryId { get; set; }
}