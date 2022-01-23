
namespace Organizer3.Controllers
{
    public class UserAccessDataEditModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public bool Recruter { get; set; } //--
        public bool Scheduler { get; set; }//--
        public bool LeaveEditor { get; set; }//--
        public bool UserEditor { get; set; }
        public bool UserViewer { get; set; }//--
        public bool FacilityEditor { get; set; }
        public bool FacilityViewer { get; set; }//--
        public bool Announcer { get; set; }//--
        public bool PersonalViewer { get; set; }//--
        public bool PartnerViewer { get; set; }//--

        public UserAccessDataEditModel()
        {
        }
    }
}