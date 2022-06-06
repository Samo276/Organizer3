using System.ComponentModel.DataAnnotations;

namespace Organizer3.Models.Scheduler
{
    public class Timeframe
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Wymagane")]
        public DateTime From { get; set; }
        [Required(ErrorMessage = "Wymagane")]
        public DateTime To { get; set; }
    }
}
