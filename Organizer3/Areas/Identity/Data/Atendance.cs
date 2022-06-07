using System.ComponentModel.DataAnnotations.Schema;

namespace Organizer3.Areas.Identity.Data
{
    public class Atendance
    {
        public int Id { get; set; }
        [ForeignKey("ShiftInfo")]
        public int ShiftId { get; set; }
        [ForeignKey("AppUser")]
        public string UserId { get; set; }
        public DateTime ShiftDate { get; set;}
        public DateTime? EnetrTime { get; set;}
        public DateTime? ExitTime { get; set; }
    }
}
