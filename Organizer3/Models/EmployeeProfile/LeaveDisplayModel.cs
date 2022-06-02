using System.ComponentModel.DataAnnotations.Schema;

namespace Organizer3.Models.EmployeeProfile
{
    public class LeaveDisplayModel
    {
        public int Id { get; set; }
        public string LeaveType { get; set; }
        public DateTime LeaveStart { get; set; }
        public DateTime LeaveEnd { get; set; }
        public string AuthorizerId { get; set; }
        public string AuthorizerName { get; set; }
        public string Note { get; set; }
    }
}
