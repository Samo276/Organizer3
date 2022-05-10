using Microsoft.AspNetCore.Mvc;
using Organizer3.Areas.Identity.Data;
using Organizer3.Data;
using Organizer3.Models;
using Organizer3.Models.Enums;

namespace Organizer3.Controllers
{
    public class JobAplicationController : Controller
    {
        private readonly OrganizerDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public JobAplicationController(OrganizerDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> JobAplicationIndex()
        {
            return View();
        }
        public async Task<IActionResult> JobAplicationConfirmation()
        {
            return View();
        }
        public async Task<IActionResult> SendJobAplication(JobAplicationModel p)
        {
            if(ModelState.IsValid)
            {
                string fileName = Path.GetFileName(p.Resume.FileName);
                string path = Path.Combine(_environment.WebRootPath, "uploads");
                Guid guid = Guid.NewGuid();
                string FileDirectory = Path.Combine(path, guid.ToString());
                string OriginalFilePath = Path.Combine(path, guid.ToString(), fileName);
                Directory.CreateDirectory(FileDirectory);
                using (FileStream stream = new(OriginalFilePath, FileMode.Create))
                {
                    await p.Resume.CopyToAsync(stream);
                }

                var tmp = new Recruitment
                {
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Email = p.Email,
                    PhoneNumber = p.PhoneNumber,
                    ResumeLocation = string.Concat("/uploads/", guid.ToString(), "/" + fileName),
                    Position = p.Position,
                    Status = RecruterEnum.RecruterEnumData.Applied.ToString(),
                    AppliedAt = DateTime.Now
                };

                _context.Recruitments.Add(tmp);
                await _context.SaveChangesAsync();

                /*var tmp = _context.Users.FirstOrDefault(x => x.Id == p.Id);
                var oldphoto = new FileInfo(_environment.WebRootPath + "/" + tmp.PhotoLocation);
                if (oldphoto.Exists)
                {
                    oldphoto.Delete();
                    oldphoto.Directory.Delete();

                }
                tmp.PhotoLocation = string.Concat("/uploads/", guid.ToString(), "/" + fileName);
                //tmp.PhotoLocation = string.Concat("/uploads/", + fileName);
                _context.Users.Update(tmp);*/

                return RedirectToAction(nameof(JobAplicationConfirmation));
            }
            return RedirectToAction(nameof(JobAplicationIndex));
        }
    }
}
