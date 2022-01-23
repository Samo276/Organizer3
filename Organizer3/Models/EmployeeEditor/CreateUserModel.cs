namespace Organizer3.Models.EmployeeEditor
{
    public class CreateUserModel
    {
        public bool Recruter { get; set; } 
        public bool Scheduler { get; set; }
        public bool LeaveEditor { get; set; }
        public bool UserEditor { get; set; }
        public bool UserViewer { get; set; }
        public bool FacilityEditor { get; set; }
        public bool FacilityViewer { get; set; }
        public bool Announcer { get; set; }
        public bool PersonalViewer { get; set; }
        public bool PartnerViewer { get; set; }
        //-----------------------------------------------
        public string? FirstName { get; set; }
        public string? SecondaryName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Street { get; set; }
        public string? ApartmentNumber { get; set; }
        //-----------------------------------------------
        public string? Ocupation { get; set; }
        public DateTime? EmployedSince { get; set; }
        public string? ContractType { get; set; }
        public DateTime? ContractExpiration { get; set; }
        public string? SupervisorId { get; set; }
        public string? otherInfo { get; set; }

        public CreateUserModel()
        {
        }
    }
}
