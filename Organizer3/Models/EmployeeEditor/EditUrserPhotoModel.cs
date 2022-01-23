namespace Organizer3.Models.EmployeeEditor
{
    public class EditUrserPhotoModel
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? PhotoLocation { get; set; }
        public IFormFile Photo { get; set; }
    }
}
