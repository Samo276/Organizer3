using System.ComponentModel.DataAnnotations.Schema;

namespace Organizer3.Areas.Identity.Data
{
    public class EmploymentStatus
    {
        public int Id { get; set; }
        public string UserId { get;set; }
        public bool IsEmployed { get; set; }    
        public virtual AppUser User { get; set; }
        public string ? Ocupation { get; set; }
        [ForeignKey("Facility")]       
        public int? FacilityId { get; set; }
        public virtual Facility Facility { get; set; }
        
        public DateTime ? EmployedSince { get; set; }
        public string ? ContractType { get; set; }
        public DateTime ? ContractExpiration { get; set; }
        public string ? SupervisorId { get; set; }
        public string? otherInfo { get;set;}
    }
}