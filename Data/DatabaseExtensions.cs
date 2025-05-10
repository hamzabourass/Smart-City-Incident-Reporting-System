using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using SCIRS.Enums;
using SCIRS.Models;

namespace SCIRS.Data;

public static class DatabaseExtensions
{
   public static async Task MigrateDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<ScirsContext>(); 
        await dbContext.Database.MigrateAsync();
    }
    
    public static async Task SeedTestDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ScirsContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        
        // Check if we already have data
        if (await dbContext.Cities.AnyAsync() || await dbContext.Reports.AnyAsync())
        {
            // Data already exists, skip seeding
            return;
        }
        
        // Create roles if they don't exist
        string[] roles = { "Admin", "Reporter", "Resolver" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
        
        // Create cities
        var casablanca = new City
        {
            Name = "Casablanca",
            Area = null // Simple approximation
        };
        
        var rabat = new City
        {
            Name = "Rabat",
            Area = null// Simple approximation
        };
        
        await dbContext.Cities.AddRangeAsync(casablanca, rabat);
        await dbContext.SaveChangesAsync();
        
        // Create users
        var adminUser = new User
        {
            UserName = "admin@example.com",
            Email = "admin@example.com",
            CityId = casablanca.Id,
            Role = "Admin",
            EmailConfirmed = true
        };
        
        var reporterUser = new User
        {
            UserName = "reporter@example.com",
            Email = "reporter@example.com",
            CityId = casablanca.Id,
            Role = "Reporter",
            EmailConfirmed = true
        };
        
        var resolverUser = new User
        {
            UserName = "resolver@example.com",
            Email = "resolver@example.com",
            CityId = rabat.Id,
            Role = "Resolver",
            EmailConfirmed = true
        };
        
        const string defaultPassword = "P@ssw0rd123";
        
        if (await userManager.FindByEmailAsync(adminUser.Email) == null)
        {
            await userManager.CreateAsync(adminUser, defaultPassword);
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
        
        if (await userManager.FindByEmailAsync(reporterUser.Email) == null)
        {
            await userManager.CreateAsync(reporterUser, defaultPassword);
            await userManager.AddToRoleAsync(reporterUser, "Reporter");
        }
        
        if (await userManager.FindByEmailAsync(resolverUser.Email) == null)
        {
            await userManager.CreateAsync(resolverUser, defaultPassword);
            await userManager.AddToRoleAsync(resolverUser, "Resolver");
        }
        
        // Create reports
        var report1 = new Report
        {
            Title = "Pothole on Main Street",
            Description = "Large pothole causing traffic issues",
            ImageUrl = "https://example.com/images/pothole1.jpg", // Placeholder URL
            Location = new Point(-7.5900, 33.5731) { SRID = 4326 }, // Near Casablanca
            Status = ReportStatus.Pending,
            CreatedAt = DateTime.UtcNow.AddDays(-5),
            ResolvedAt = DateTime.MinValue, // Not resolved yet
            CityId = casablanca.Id,
            City = casablanca,
            CreatedById = reporterUser.Id,
            CreatedBy = reporterUser
        };
        
        var report2 = new Report
        {
            Title = "Broken Street Light",
            Description = "Street light not working at the corner of 5th and Main",
            ImageUrl = "https://example.com/images/light1.jpg", // Placeholder URL
            Location = new Point(-7.5890, 33.5735) { SRID = 4326 }, // Near Casablanca
            Status = ReportStatus.InProgress,
            CreatedAt = DateTime.UtcNow.AddDays(-3),
            ResolvedAt = DateTime.MinValue, // Not resolved yet
            CityId = casablanca.Id,
            City = casablanca,
            CreatedById = reporterUser.Id,
            CreatedBy = reporterUser,
            AssignedToId = resolverUser.Id,
            AssignedTo = resolverUser
        };
        
        var report3 = new Report
        {
            Title = "Garbage Not Collected",
            Description = "Garbage bins are overflowing on Avenue Hassan II",
            ImageUrl = "https://example.com/images/garbage1.jpg", // Placeholder URL
            Location = new Point(-6.8420, 34.0210) { SRID = 4326 }, // Near Rabat
            Status = ReportStatus.Resolved,
            CreatedAt = DateTime.UtcNow.AddDays(-7),
            ResolvedAt = DateTime.UtcNow.AddDays(-1), // Resolved recently
            CityId = rabat.Id,
            City = rabat,
            CreatedById = reporterUser.Id,
            CreatedBy = reporterUser,
            AssignedToId = resolverUser.Id,
            AssignedTo = resolverUser
        };
        
        await dbContext.Reports.AddRangeAsync(report1, report2, report3);
        
        // Create report status history
        var history1 = new ReportStatusHistory
        {
            Status = ReportStatus.Pending,
            Timestamp = report1.CreatedAt,
            ReportId = report1.Id,
            Report = report1,
            ChangedById = reporterUser.Id,
            ChangedBy = reporterUser
        };
        
        var history2a = new ReportStatusHistory
        {
            Status = ReportStatus.Pending,
            Timestamp = report2.CreatedAt,
            ReportId = report2.Id,
            Report = report2,
            ChangedById = reporterUser.Id,
            ChangedBy = reporterUser
        };
        
        var history2b = new ReportStatusHistory
        {
            Status = ReportStatus.InProgress,
            Timestamp = report2.CreatedAt.AddHours(5),
            ReportId = report2.Id,
            Report = report2,
            ChangedById = resolverUser.Id,
            ChangedBy = resolverUser
        };
        
        var history3a = new ReportStatusHistory
        {
            Status = ReportStatus.Pending,
            Timestamp = report3.CreatedAt,
            ReportId = report3.Id,
            Report = report3,
            ChangedById = reporterUser.Id,
            ChangedBy = reporterUser
        };
        
        var history3b = new ReportStatusHistory
        {
            
            Status = ReportStatus.InProgress,
            Timestamp = report3.CreatedAt.AddDays(1),
            ReportId = report3.Id,
            Report = report3,
            ChangedById = resolverUser.Id,
            ChangedBy = resolverUser
        };
        
        var history3c = new ReportStatusHistory
        {
            Status = ReportStatus.Resolved,
            Timestamp = report3.ResolvedAt,
            ReportId = report3.Id,
            Report = report3,
            ChangedById = resolverUser.Id,
            ChangedBy = resolverUser
        };
        
        await dbContext.ReportStatusHistories.AddRangeAsync(history1, history2a, history2b, history3a, history3b, history3c);
        
        await dbContext.SaveChangesAsync();
    }
}