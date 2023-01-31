using Api.DbContextes;
using Api.Entities;
using Api.Interfaces;

namespace Api.Repository;

public class HotelRepository : GenericRepository<Hotel>, IHotelsRepository
{
    public HotelRepository(HotelListingDbContext context) : base(context)
    {
    }
}