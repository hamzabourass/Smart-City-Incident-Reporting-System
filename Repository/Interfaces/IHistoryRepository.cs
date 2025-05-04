using System;
using SCIRS.Models;

namespace SCIRS.Repository.Interfaces;

public interface IHistoryRepository : IGenericRepository<ReportStatusHistory>
{
    Task<IEnumerable<ReportStatusHistory>> GetHistoryByReportIdAsync(int reportId);
}
