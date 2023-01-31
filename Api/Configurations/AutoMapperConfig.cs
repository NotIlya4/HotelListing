﻿using Api.Data.Entity;
using Api.Models.Country;
using Api.Models.Hotel;
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
    }
}