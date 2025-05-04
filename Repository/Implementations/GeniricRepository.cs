using System;
using Microsoft.EntityFrameworkCore;
using SCIRS.Data;
using SCIRS.Repository.Interfaces;

namespace SCIRS.Repository.Implementations;

public class GeniricRepository<T> : IGenericRepository<T> where T : class
{

    protected readonly ScirsContext _context;
    protected readonly DbSet<T> _dbSet;

    public GeniricRepository(ScirsContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public virtual void Update(T entity)
    {
        _dbSet.Update(entity);
    }
}
