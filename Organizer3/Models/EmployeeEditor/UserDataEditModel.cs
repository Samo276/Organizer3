using Microsoft.EntityFrameworkCore;
using Organizer3.Areas.Identity.Data;

namespace Organizer3.Models.EmployeeEditor
{
    public class UserDataEditModel
    {
        
        public string UserId { get; set; }
        public string? FirstName { get; set; }
        public string? SecondaryName { get; set; }
        public string? LastName { get; set; }
        public string ? PhoneNo { get; set; }
        public string? Email { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Street { get; set; }
        public string? ApartmentNumber { get; set; }

        public UserDataEditModel(AppUser appUserData)
        {
            UserId = appUserData.Id;
            FirstName = appUserData.FirstName;
            SecondaryName = appUserData.SecondaryName;
            LastName = appUserData.LastName;
            Email = appUserData.Email;
            City = appUserData.City;
            PhoneNo = appUserData.PhoneNumber;
            PostalCode = appUserData.PostalCode;
            Street = appUserData.Street;
            ApartmentNumber = appUserData.ApartmentNumber;            
        }

        public UserDataEditModel()
        {
        }
    }
}
