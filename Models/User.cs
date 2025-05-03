using System;
using Microsoft.AspNetCore.Identity;

namespace SCIRS.Models;

public class User :  IdentityUser
{
    public int CityId { get; set;}
    public City? City { get; set;}

    public required string Role { get; set; }

}
