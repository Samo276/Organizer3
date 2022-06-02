using System.ComponentModel.DataAnnotations;

namespace Organizer3.Models.EmployeeProfile
{
    public class AddNewLeaveModel
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string UserId { get; set; }
        [Required(ErrorMessage = "Nie wybrano typu urlopu.")]
        public string LeaveType { get; set; }
        [Required(ErrorMessage = "Nie wybrano daty urlopu.")]
        public DateTime LeaveStart { get; set; }
        [Required(ErrorMessage = "Nie wybrano czasu trwania urlopu.")]
        public int LeaveDuration { get; set; }
        public string ? Note { get; set; }
    }
}
