namespace Organizer3.Areas.Identity.Data
{
    public class Recruitment
    {
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ResumeLocation { get; set; }
        public string Position { get; set; }
        public string Status { get; set; }
        public DateTime AppliedAt { get; set; }
        public string ? Notes { get;set; }
        public List<RecruitmentNotes> ? Recruit_Notes { get; set; }
    }
}
