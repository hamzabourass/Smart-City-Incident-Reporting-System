using System;
using Microsoft.EntityFrameworkCore;
using SCIRS.Data;
using SCIRS.Models;
using SCIRS.Repository.Interfaces;

namespace SCIRS.Repository.Implementations;

public class CityRepository : GeniricRepository<City>, ICityRepository
{
    public CityRepository(ScirsContext context) : base(context)
    {
    }

    public async Task<City?> GetByNameAsync(string name)
    {
        return await _context.Cities
                            .FirstOrDefaultAsync(c => string.Equals(
                                c.Name, 
                                name, 
                                StringComparison.OrdinalIgnoreCase
                            ));
    }

    public async Task<IEnumerable<City>> GetCitiesByRegionAsync(string region)
        {
            // This is a placeholder implementation
            // You'll need to modify this once you add region to your City model
            return await _context.Cities.ToListAsync();
        }

    public async Task<City?> SearchByName(string name)
    {
        return await _context.Cities.FirstOrDefaultAsync(c => c.Name == name);
    }
}
