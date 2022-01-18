using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Organizer3.Areas.Identity.Data;

// Add profile data for application users by adding properties to the AppUser class
public class AppUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string ? SecondaryName { get; set; }
    public string ? LastName { get; set; }
    public string ? AltEmail { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public string? Street { get; set; }
    public string? ApartmentNumber { get; set; }
    public string? PhotoLocation { get; set; }
    public virtual UserAccess Accesses { get; set; }
    [ForeignKey("EmploymentStatus")]
    public int EmploymentStatusId { get; set; }
    public virtual EmploymentStatus EmploymentStatus { get; set; }


}

