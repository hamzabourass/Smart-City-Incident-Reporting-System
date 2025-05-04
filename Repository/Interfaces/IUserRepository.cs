using System;
using SCIRS.Models;

namespace SCIRS.Repository.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}

