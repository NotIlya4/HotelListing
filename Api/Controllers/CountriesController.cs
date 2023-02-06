using Api.Dtos.Country;
using Api.Entities;
using Api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountriesController : ControllerBase
{
    private readonly ICountriesRepository _countriesRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CountriesController> _logger;

    public CountriesController(ICountriesRepository countriesRepository, IMapper mapper, ILogger<CountriesController> logger)
    {
        _countriesRepository = countriesRepository;
        _mapper = mapper;
        _logger = logger;
    }

    // GET: api/Countries
    [HttpGet]
    public async Task<ActionResult<List<GetCountryDto>>> GetCountries()
    {
        var countries = await _countriesRepository.GetAllAsync();
        List<GetCountryDto> records = _mapper.Map<List<GetCountryDto>>(countries);
        return Ok(records);
    }

    // GET: api/Countries/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CountryDto>> GetCountry(int id)
    {
        Country? country = await _countriesRepository.GetDetails(id);

        if (country is null)
        {
            _logger.LogWarning("No record found in {0} with id={1}", nameof(GetCountry), id);
            return NotFound();
        };

        CountryDto record = _mapper.Map<CountryDto>(country);

        return Ok(record);
    }

    // PUT: api/Countries/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountryDto)
    {
        if (id != updateCountryDto.Id)
        {
            return BadRequest();
        }

        Country country = _mapper.Map<Country>(updateCountryDto);
        country.Id = id;

        await _countriesRepository.UpdateAsync(country);

        return NoContent();
    }

    // POST: api/Countries
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<GetCountryDto>> PostCountry(CreateCountryDto createCountry)
    {
        Country country = _mapper.Map<Country>(createCountry);
        country = await _countriesRepository.AddAsync(country);

        GetCountryDto getCountryDto = _mapper.Map<GetCountryDto>(country);

        return CreatedAtAction("GetCountry", new { id = country.Id }, getCountryDto);
    }

    // DELETE: api/Countries/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        await _countriesRepository.DeleteAsync(id);

        return NoContent();
    }
}