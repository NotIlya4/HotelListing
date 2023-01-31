using Api.Data.Entity;

namespace Api.Interfaces;

public interface ICountriesRepository : IGenericRepository<Country>
{
    public Task<Country?> GetDetails(int id);
}