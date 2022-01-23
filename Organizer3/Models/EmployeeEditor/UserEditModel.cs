using Microsoft.EntityFrameworkCore;
using Organizer3.Areas.Identity.Data;

namespace Organizer3.Models
{
    public class UserEditModel
    {
        public string UserId { get; set; }
        public string? FirstName { get; set; }
        public string? SecondaryName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Street { get; set; }
        public string? ApartmentNumber { get; set; }
        public string? PhotoLocation { get; set; }
        public UserAccess? userAccessData { get; set; }
        public EmploymentStatus? EmploymentStatusData { get; set; }
        public Facility FacilityData { get; set; }
        public UserEditModel(AppUser appUserData, UserAccess accessData, EmploymentStatus employment, Facility facilityData)
        {
            UserId = appUserData.Id;
            FirstName = appUserData.FirstName;
            SecondaryName = appUserData.SecondaryName;
            LastName = appUserData.LastName;
            Email = appUserData.Email;
            City = appUserData.City;
            PostalCode = appUserData.PostalCode;
            Street = appUserData.Street;
            ApartmentNumber = appUserData.ApartmentNumber;
            PhotoLocation = appUserData.PhotoLocation;
            userAccessData = accessData;
            EmploymentStatusData = employment;
            FacilityData = facilityData;
        }

        public UserEditModel()
        {
        }
    }

}
