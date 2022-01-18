namespace Organizer3.Areas.Identity.Data
{
    public class UserViewerModel
    {
        public int id;
        public AppUser AppUserData { get; set; }
        public UserAccess userAccessData { get; set; }
        public EmploymentStatus EmploymentStatusData { get; set; }
    }
}
