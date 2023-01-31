using Api.Data.Context;
using Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly HotelListingDbContext _context;

    public GenericRepository(HotelListingDbContext context)
    {
        _context = context;
    }

    public DbSet<T> GetSet()
    {
        return _context.Set<T>();
    }
    
    public async Task<T?> GetAsync(int id)
    {
        return await GetSet().FindAsync(id);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await GetSet().ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        var entityEntry = await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task DeleteAsync(int id)
    {
        T? entity = await GetAsync(id);
        if (entity is not null)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> Exists(int id)
    {
        T? entity = await GetAsync(id);
        return entity is not null;
    }
}