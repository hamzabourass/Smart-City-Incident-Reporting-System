using System;
using SCIRS.Models;

namespace SCIRS.Repository.Interfaces;

public interface ICityRepository : IGenericRepository<City>
{
    Task<City?> GetByNameAsync(string name);
    Task<IEnumerable<City>> GetCitiesByRegionAsync(string region);
}
