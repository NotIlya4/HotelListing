using Api.Dtos.Hotel;

namespace Api.Dtos.Country;

public class CountryDto : BaseCountryDto
{
    public required int Id { get; set; }
    public required List<HotelDto> Hotels { get; set; }
}