using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organizer3.Areas.Identity.Data
{
    public class RecruitmentNotes
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        [ForeignKey("Recruitment")]
        public int ? RecruitmentId { get; set; }
        public string ? NoteAuthor { get; set; }        
        public string ? NoteContent { get; set; }
    }
}
