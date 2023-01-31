using Api.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Context;

public class HotelListingDbContext : DbContext
{
    public HotelListingDbContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<Hotel> Hotels { get; set; } = null!;
    public DbSet<Country> Countries { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>().HasData(
            new Country()
            {
                CountryId = 1,
                Name = "Jamaica",
                ShortName = "JM"
            },
            new Country()
            {
                CountryId = 2,
                Name = "Bahamas",
                ShortName = "BS"
            },
            new Country()
            {
                CountryId = 3,
                Name = "Cayman Islands",
                ShortName = "CI"
            });

        modelBuilder.Entity<Hotel>().HasData(
            new Hotel()
            {
                HotelId = 1,
                Name = "Sandals Resort and Spa",
                Address = "Negril",
                CountryId = 1,
                Rating = 4.5
            },
            new Hotel()
            {
                HotelId = 2,
                Name = "Comfort Suites",
                Address = "George Town",
                CountryId = 3,
                Rating = 4.3
            },
            new Hotel()
            {
                HotelId = 3,
                Name = "Grand Palladiun",
                Address = "Nassua",
                CountryId = 2,
                Rating = 4
            });
    }
}