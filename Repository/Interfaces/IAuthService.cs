using System;
using SCIRS.Models;

namespace SCIRS.Repository.Interfaces;

public interface IAuthService
{
    Task<string> GenerateJwtTokenAsync(User user);
    Task<User?> ValidateUserCredentialsAsync(string email, string password);
}

