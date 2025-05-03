using System;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;
using SCIRS.Enums;

namespace SCIRS.Models;

public class Report
{
    public int Id { get; set;}
    public required string Title { get; set;}
    public string? Description { get; set;}

    public required string ImageUrl { get; set;}
    public required Point Location { get; set;}

    public required ReportStatus Status { get; set;}

    public DateTime CreatedAt { get; set;}
    public DateTime ResolvedAt { get; set;}

    public required int CityId { get; set;}
    public required City City { get; set; }

    public required string CreatedById { get; set;}
    [ForeignKey(nameof(CreatedById))]
    public required User CreatedBy { get; set; }

    public string? AssignedToId { get; set;}
    [ForeignKey(nameof(AssignedToId))]
    public User? AssignedTo { get; set; }

    public ICollection<ReportStatusHistory>? StatusHistories { get; set; }




}
