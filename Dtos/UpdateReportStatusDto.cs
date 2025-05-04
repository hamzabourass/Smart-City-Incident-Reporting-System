using System;
using System.ComponentModel.DataAnnotations;
using SCIRS.Enums;

namespace SCIRS.Dtos;

    public class UpdateReportStatusDto
    {
        [Required]
        public ReportStatus Status { get; set; }
    }
