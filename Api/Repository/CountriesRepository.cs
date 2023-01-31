using Api.Data.Context;
using Api.Data.Entity;
using Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repository;

public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
{
    private readonly HotelListingDbContext _context;

    public CountriesRepository(HotelListingDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Country?> GetDetails(int id)
    {
        return await _context.Countries
            .Include(p => p.Hotels)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}