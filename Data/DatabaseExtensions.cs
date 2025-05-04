using System;
using Microsoft.EntityFrameworkCore;

namespace SCIRS.Data;

public static class DatabaseExtensions
{
   public static async Task MigrateDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<ScirsContext>(); 
        await dbContext.Database.MigrateAsync();

    }
}
