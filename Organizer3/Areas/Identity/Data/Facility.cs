namespace Organizer3.Areas.Identity.Data
{
    public class Facility
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Adress { get; set; }
        public string PhoneNo { get; set; }
        public string AdditionalInfo { get; set; }
        public ICollection<EmploymentStatus> Employments { get; set; }
    }
}
