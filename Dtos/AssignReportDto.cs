using System;
using System.ComponentModel.DataAnnotations;

namespace SCIRS.Dtos;

   public class AssignReportDto
    {
        [Required]
        public string AssignedToId { get; set; } = string.Empty;
    }
