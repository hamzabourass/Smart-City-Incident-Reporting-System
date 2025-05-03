using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SCIRS.Models;

namespace SCIRS.Data;

public class ScirsContext : IdentityDbContext<User>
{
    public ScirsContext(DbContextOptions<ScirsContext> options) : base(options)
    {
    }

    public DbSet<City> Cities { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<ReportStatusHistory> ReportStatusHistories { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<City>()
            .Property(c => c.Area)
            .HasColumnType("geography");

        modelBuilder.Entity<Report>()
            .Property(r => r.Location)
            .HasColumnType("geography");

    modelBuilder.Entity<Report>()
        .HasOne(r => r.City)
        .WithMany(c => c.Reports)
        .HasForeignKey(r => r.CityId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<ReportStatusHistory>()
        .HasOne(rsh => rsh.Report)
        .WithMany()
        .HasForeignKey(rsh => rsh.ReportId)
        .OnDelete(DeleteBehavior.Restrict); 

    modelBuilder.Entity<ReportStatusHistory>()
        .HasOne(rsh => rsh.ChangedBy)
        .WithMany()
        .HasForeignKey(rsh => rsh.ChangedById)
        .OnDelete(DeleteBehavior.Restrict); 
    }
}
