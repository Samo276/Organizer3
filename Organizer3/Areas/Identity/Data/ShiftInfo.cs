using System.ComponentModel.DataAnnotations.Schema;

namespace Organizer3.Areas.Identity.Data
{
    public class ShiftInfo
    {
        public int Id { get; set; }
        public string ShiftName { get; set; }    
        [ForeignKey("Facility")]
        public int FacilityId { get; set; }
        public DateTime StartingTime { get; set; }
        public int Duration { get; set; }
        public bool Archived { get; set; } = false;
    }
}
