using Api.Models.Hotel;

namespace Api.Models.Country;

public class CountryDto : BaseCountryDto
{
    public required int Id { get; set; }
    public required List<HotelDto> Hotels { get; set; }
}