using Organizer3.Areas.Identity.Data;

namespace Organizer3.Models
{
    public class EmploymentStatusModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public bool IsEmployed { get; set; }
        public string? Ocupation { get; set; }              
        public DateTime? EmployedSince { get; set; }
        public string? ContractType { get; set; }
        public DateTime? ContractExpiration { get; set; }
        public string? SupervisorId { get; set; }
        public string? otherInfo { get; set; }

        public EmploymentStatusModel()
        {
        }
    }
}
