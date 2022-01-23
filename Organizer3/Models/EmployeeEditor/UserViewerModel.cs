namespace Organizer3.Areas.Identity.Data
{
    public class UserViewerModel
    {
        
        public string UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Ocupation { get; set; }
        public string? FacilityName { get; set; }

        public UserViewerModel(string userId, string? firstName, string? lastName, string? ocupation, string? facilityName)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Ocupation = ocupation;
            FacilityName = facilityName;
        }
    }
}
