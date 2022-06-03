using Microsoft.AspNetCore.Mvc;
using Organizer3.Models.MyValidationAtributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organizer3.Models
{
    public class JobAplicationModel
    {
        public int id { get; set; }
        [Required( ErrorMessage = "Pole wymagane.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Pole wymagane.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Pole wymagane.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Pole wymagane.")]
        public string PhoneNumber { get; set; }
        [NotMapped]
        [Required]
        [MaxFileSize(5 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".pdf"})]
        public IFormFile Resume { get; set; }
        [Required( ErrorMessage = "Pole wymagane.")]
        public string Position { get; set; }

    }
}
