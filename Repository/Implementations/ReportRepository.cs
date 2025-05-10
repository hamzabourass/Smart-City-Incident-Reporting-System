using System;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using SCIRS.Data;
using SCIRS.Enums;
using SCIRS.Models;
using SCIRS.Repository.Interfaces;

namespace SCIRS.Repository.Implementations;

public class ReportRepository : GeniricRepository<Report>, IReportRepository
{
    public ReportRepository(ScirsContext context) : base(context) { }
    

    public override async Task<IEnumerable<Report>> GetAllAsync(){
        return await _context.Reports
            .Include(r => r.City)
            .Include(r => r.CreatedBy)
            .Include(r => r.AssignedTo)
            .ToListAsync();
    }

    public override async Task<Report?> GetByIdAsync(int id){
        return await _context.Reports
            .Include(r => r.City)
            .Include(r => r.CreatedBy)
            .Include(r => r.AssignedTo)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Report>> GetDeletedReportsAsync()
    {
        return await _context.Reports
            .Where(r => r.IsDeleted)
            .Include(r => r.City)
            .Include(r => r.CreatedBy)
            .Include(r => r.AssignedTo)
            .Include(r => r.DeletedBy)
            .ToListAsync();
    }
    
    public async Task<Report?> GetReportByIdIncludingDeletedAsync(int id)
    {
        return await _context.Reports
            .Include(r => r.City)
            .Include(r => r.CreatedBy)
            .Include(r => r.AssignedTo)
            .Include(r => r.DeletedBy)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Report>> GetReportsByCityId(int cityId)
    {
        return await _context.Reports
            .Include(r => r.CreatedBy)
            .Include(r => r.AssignedTo)
            .Where(r => r.CityId == cityId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Report>> GetReportsByStatusAsync(string status)
    {
        if(Enum.TryParse<ReportStatus>(status, true, out var reportStatus))
        {
            return await _context.Reports
                .Include(r => r.City)
                .Include(r => r.CreatedBy)
                .Include(r => r.AssignedTo)
                .Where(r => r.Status == reportStatus)
                .ToListAsync();

        }

        return [];
    }

    public async Task<IEnumerable<Report>> GetReportsByUserId(string id)
    {
        return await _context.Reports
            .Include(r => r.City)
            .Include(r => r.AssignedTo)
            .Where(r => r.CreatedById == id)
            .ToListAsync();
    }

    public async Task<IEnumerable<Report>> GetReportsNearLocationAsync(double latitude, double longitude, double radiusKm)
    {
       var point = new Point(longitude, latitude) { SRID = 4326 };

        return await _context.Reports
            .Include(r => r.City)
            .Include(r => r.CreatedBy)
            .Include(r => r.AssignedTo)
            .Where(r => r.Location.Distance(point) < radiusKm)
            .ToListAsync();
    }
}
