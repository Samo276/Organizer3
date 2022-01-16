namespace Organizer3.Areas.Identity.Data
{
    public class UserAccess
    {
        public int Id { get; set; }
        public virtual AppUser User { get; set; }
        public string UserId { get; set; }
        public bool Recruter { get; set; }
        public bool Scheduler { get; set; }
        public bool LeaveEditor { get; set; }
        public bool UserEditor { get; set; }
        public bool UserViewer { get; set; }
        public bool FacilityEditor { get; set; }
        public bool FacilityViewer { get; set; }
        public bool Announcer { get; set; }
        public bool PersonalViewer { get; set; }
        public bool PartnerViewer { get; set; }

    }
}
