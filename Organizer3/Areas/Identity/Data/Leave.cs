using System.ComponentModel.DataAnnotations.Schema;

namespace Organizer3.Areas.Identity.Data
{
    public class Leave
    {
        public int Id { get; set; }        
        [ForeignKey("AppUser")]
        public string UserId { get; set; }
        public string LeaveType { get; set; }
        public DateTime LeaveStart { get; set; }
        public int LeaveDuration { get; set; }
        public string AuthorizerId { get; set; }
        public string Note { get; set; }

    }
}
