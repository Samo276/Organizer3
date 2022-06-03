using System.ComponentModel.DataAnnotations;

namespace Organizer3.Models.MyShop
{
    public class AddShiftModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Range(0, 23, ErrorMessage = "Godzina rozpoczęcia musi zawierać się między godzinami 00 a 23")]
        public int StartingHour { get; set; }
        [Range(0, 24, ErrorMessage = "Czas trwania zmiany musi zawierać się między 0 a 24 godzin")]
        public int Duration { get; set; }
    }
}
