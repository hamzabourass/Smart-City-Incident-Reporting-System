using System;
using SCIRS.Models;

namespace SCIRS.Repository.Interfaces;

public interface IReportRepository : IGenericRepository<Report>
{

    Task<IEnumerable<Report>> GetReportsByCityId(int id);
    Task<IEnumerable<Report>> GetReportsByUserId(string id);
    Task<IEnumerable<Report>> GetReportsByStatusAsync(string status);
    Task<IEnumerable<Report>> GetReportsNearLocationAsync(double latitude, double longitude, double radiusKm);
    Task<IEnumerable<Report>> GetDeletedReportsAsync();
    Task<Report> GetReportByIdIncludingDeletedAsync(int id);

}
