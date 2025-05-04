using System;
using System.ComponentModel.DataAnnotations;

namespace SCIRS.Dtos;

 public class CreateReportDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        [Required]
        public double Latitude { get; set; }
        
        [Required]
        public double Longitude { get; set; }
        
        [Required]
        public int CityId { get; set; }
    }