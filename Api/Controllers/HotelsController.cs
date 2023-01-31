using Microsoft.AspNetCore.Mvc;
using Api.Dtos.Hotel;
using Api.Entities;
using Api.Interfaces;
using AutoMapper;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelsRepository _hotelsRepository;
        private readonly IMapper _mapper;

        public HotelsController(IHotelsRepository hotelsRepository, IMapper mapper)
        {
            _hotelsRepository = hotelsRepository;
            _mapper = mapper;
        }

        // GET: api/Hotels
        [HttpGet]
        public async Task<ActionResult<List<HotelDto>>> GetHotels()
        {
            List<Hotel> hotels = await _hotelsRepository.GetAllAsync();
            return Ok(_mapper.Map<List<HotelDto>>(hotels));
        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelDto>> GetHotel(int id)
        {
            Hotel? hotel = await _hotelsRepository.GetAsync(id);

            if (hotel == null)
            {
              return NotFound();
            }

            return Ok(_mapper.Map<HotelDto>(hotel));
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, HotelDto hotelDto)
        {
            if (id != hotelDto.Id)
            {
                return BadRequest();
            }

            Hotel hotel = _mapper.Map<Hotel>(hotelDto);

            await _hotelsRepository.UpdateAsync(hotel);

            return NoContent();
        }

        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hotel>> PostHotel(CreateHotelDto createHotelDto)
        {
            Hotel hotel = _mapper.Map<Hotel>(createHotelDto);
            
            hotel = await _hotelsRepository.AddAsync(hotel);
            
            return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            await _hotelsRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
