using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using SCIRS.Dtos;
using SCIRS.Enums;
using SCIRS.Models;
using SCIRS.Repository.Interfaces;
using SCIRS.Services.Interfaces;

namespace SCIRS.Services.Implementations;

public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICityRepository _cityRepository;
    private readonly IMapper _mapper;

    public ReportService(
        IReportRepository reportRepository,
        IUserRepository userRepository,
        ICityRepository cityRepository,
        IMapper mapper)
    {
        _reportRepository = reportRepository;
        _userRepository = userRepository;
        _cityRepository = cityRepository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<ReportDto>> GetAllReportsAsync()
    {
        try
        {
            var reports = await _reportRepository.GetAllAsync();
            
            if (reports == null || !reports.Any())
            {
                return Enumerable.Empty<ReportDto>();
            }
            
            return _mapper.Map<IEnumerable<ReportDto>>(reports);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAllReportsAsync: {ex.Message}");
            throw; 
        }
    }

    public async Task<ReportDto?> GetReportByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid report ID", nameof(id));
        }
        
        try
        {
            var report = await _reportRepository.GetByIdAsync(id);
            if (report == null)
            {
                return null;
            }

            return _mapper.Map<ReportDto>(report);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetReportByIdAsync: {ex.Message}");
            throw;
        }
    }
    public Task<bool> AssignReportAsync(int reportId, string assignedToId)
    {
        throw new NotImplementedException();
    }

    public Task<ReportDto> CreateReportAsync(CreateReportDto createReportDto, string userId, string imageUrl)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ReportDto>> GetReportsByCityIdAsync(int cityId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateReportStatusAsync(int reportId, ReportStatus status, string userId)
    {
        throw new NotImplementedException();
    }

     public async Task<bool> DeleteReportAsync(int reportId)
    {
        var report = await _reportRepository.GetByIdAsync(reportId);
        if (report == null)
            return false;
            
        _reportRepository.Delete(report);
        return await _reportRepository.SaveChangesAsync();
    }
    
    public async Task<bool> SoftDeleteReportAsync(int reportId, string deletedById)
    {
        var report = await _reportRepository.GetByIdAsync(reportId);
        if (report == null)
            return false;
            
        var user = await _userRepository.GetByIdAsync(int.Parse(deletedById));
        if (user == null)
            return false;
        
        report.IsDeleted = true;
        report.DeletedAt = DateTime.UtcNow;
        report.DeletedById = deletedById;
        
        _reportRepository.Update(report);
        return await _reportRepository.SaveChangesAsync();
    }
    
    public async Task<bool> RestoreReportAsync(int reportId)
    {
        var report = await _reportRepository.GetByIdAsync(reportId);
        if (report == null || !report.IsDeleted)
            return false;
            
        report.IsDeleted = false;
        report.DeletedAt = null;
        report.DeletedById = null;
        
        _reportRepository.Update(report);
        return await _reportRepository.SaveChangesAsync();
    }
}