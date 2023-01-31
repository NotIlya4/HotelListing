using Api.Entities;

namespace Api.Interfaces;

public interface ICountriesRepository : IGenericRepository<Country>
{
    public Task<Country?> GetDetails(int id);
}