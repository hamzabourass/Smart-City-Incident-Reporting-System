using System;

namespace SCIRS.Dtos;

 public class ReportDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        
        public int CityId { get; set; }
        public string CityName { get; set; } = string.Empty;
        
        public string CreatedById { get; set; } = string.Empty;
        public string CreatedByName { get; set; } = string.Empty;
        
        public string? AssignedToId { get; set; }
        public string? AssignedToName { get; set; }
    }