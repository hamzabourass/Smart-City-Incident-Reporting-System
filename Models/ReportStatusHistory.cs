using System;
using SCIRS.Enums;

namespace SCIRS.Models;

public class ReportStatusHistory
{
    public required int Id { get; set;}
    public ReportStatus Status {get; set;}
    public DateTime Timestamp {get; set;}

    public required int ReportId {get; set;}
    public required Report Report {get; set;}

    public required string ChangedById {get; set;}
    public required User ChangedBy {get; set;}







}
