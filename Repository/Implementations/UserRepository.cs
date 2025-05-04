using System;
using Microsoft.EntityFrameworkCore;
using SCIRS.Data;
using SCIRS.Models;
using SCIRS.Repository.Interfaces;

namespace SCIRS.Repository.Implementations;

public class UserRepository : GeniricRepository<User>, IUserRepository
{
    public UserRepository(ScirsContext context) : base(context)
    {
    }

    public override async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.City)
            .FirstOrDefaultAsync(u => u.Id == id.ToString());
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.Include(u => u.City)
        .FirstOrDefaultAsync(u => string.Equals(
            u.Email, email,
            StringComparison.OrdinalIgnoreCase));
    }
}
