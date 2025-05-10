using System;
using SCIRS.Dtos;
using SCIRS.Enums;

namespace SCIRS.Services.Interfaces;

public interface IReportService
    {
        Task<IEnumerable<ReportDto>> GetAllReportsAsync();
        Task<ReportDto?> GetReportByIdAsync(int id);
        Task<IEnumerable<ReportDto>> GetReportsByCityIdAsync(int cityId);
        Task<ReportDto> CreateReportAsync(CreateReportDto createReportDto, string userId, string imageUrl);
        Task<bool> UpdateReportStatusAsync(int reportId, ReportStatus status, string userId);
        Task<bool> AssignReportAsync(int reportId, string assignedToId);
        Task<bool> DeleteReportAsync(int reportId);
        Task<bool> SoftDeleteReportAsync(int reportId, string deletedById);
        Task<bool> RestoreReportAsync(int reportId);
}
    
