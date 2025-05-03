using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetTopologySuite.Geometries;

namespace SCIRS.Models;

public class City
{
    public int Id { get; set;}
    public required string Name { get; set;}

    public Geometry? Area { get; set;}

    public ICollection<User>? Users { get; set;}

    public ICollection<Report>? Reports { get; set;}


    

}
