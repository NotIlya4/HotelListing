using Api.Dtos.Country;
using Api.Dtos.Hotel;
using Api.Entities;
using AutoMapper;

namespace Api.Configurations;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Country, CountryDto>().ReverseMap();
        CreateMap<Country, CreateCountryDto>().ReverseMap();
        CreateMap<Country, GetCountryDto>().ReverseMap();
        CreateMap<Country, UpdateCountryDto>().ReverseMap();
        
        CreateMap<Hotel, HotelDto>().ReverseMap();
        CreateMap<Hotel, CreateHotelDto>().ReverseMap();
    }
}