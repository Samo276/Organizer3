using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organizer3.Models
{
    public class JobAplicationModel
    {
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [NotMapped]
        public IFormFile Resume { get; set; }
        public string Position { get; set; }

    }
}
