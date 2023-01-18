using System.ComponentModel.DataAnnotations;

namespace Organizer3.Models.EmployeeEditor
{
    public class CreateUserModel
    {
        [Display(Name = "Rekrutacja")]
        public bool Recruter { get; set; } 
        public bool Scheduler { get; set; }
        public bool LeaveEditor { get; set; }
        public bool UserEditor { get; set; }
        [Display(Name = "Edytor pracowników")]
        public bool UserViewer { get; set; }
        public bool FacilityEditor { get; set; }
        [Display(Name = "Edytor sklepów")]
        public bool FacilityViewer { get; set; }
        [Display(Name = "Edytor ogłoszeń")]
        public bool Announcer { get; set; }
        [Display(Name = "Przegląd danych pracowników mojej placówki")]
        public bool PersonalViewer { get; set; }
        [Display(Name = "Dodatkowe funkcje")]
        public bool PartnerViewer { get; set; }
        //-----------------------------------------------
        [Display(Name = "Imię")]
        [Required(ErrorMessage = "Pole wymagane.")]
        public string? FirstName { get; set; }
        [Display(Name = "Drugie imię")]
        public string? SecondaryName { get; set; }
        [Display(Name = "Nazwisko")]
        [Required(ErrorMessage = "Pole wymagane.")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Pole wymagane.")]
        public string Email { get; set; }
        [Display(Name = "Miasto")]
        public string? City { get; set; }
        [Display(Name = "Kod pocztowy")]
        public string? PostalCode { get; set; }
        [Display(Name = "Ulica")]
        public string? Street { get; set; }
        [Display(Name = "Numer domu/lokalu")]
        public string? ApartmentNumber { get; set; }
        //-----------------------------------------------
        [Display(Name = "Stanowisko")]
        public string? Ocupation { get; set; }
        [Display(Name = "Zatrudniony od")]
        public DateTime? EmployedSince { get; set; }
        [Display(Name = "Typ umowy")]
        public string? ContractType { get; set; }
        [Display(Name = "Data wygaśnięcia umowy")]
        public DateTime? ContractExpiration { get; set; }
        public string? SupervisorId { get; set; }
        [Display(Name = "Dodatkowe informacje")]
        public string? otherInfo { get; set; }

        public CreateUserModel()
        {
        }
    }
}
