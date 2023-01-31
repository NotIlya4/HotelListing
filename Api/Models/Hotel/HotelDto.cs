namespace Api.Models.Hotel;

public class HotelDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required double Rating { get; set; }
    public required int CountryId { get; set; }
}