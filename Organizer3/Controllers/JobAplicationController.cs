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
            if(_context.SiteFunctions.First(y => y.FunctionName == SiteFunctionsEnum.SiteFunctionsData.Recruitment.ToString()).IsActive)
                return View();

            return RedirectToAction("JobAplicationConfirmation", new {message = "Obecnie rekrutacja jest zamknięta"});
        }
        public async Task<IActionResult> JobAplicationConfirmation(string message)
        {
            List<string> messages = new List<string>();
            messages.Add(message);

            return View(messages);
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



                //mail service will not work without smtp server, you can get dummy smtp server @ https://ethereal.email/create
                //then configure data fields inside Organizer3.Areas.MailService.MailService();
                //--------------------------------------------------------------------------------
                //var MailMessage = new Organizer3.Areas.MailService.MailService();
                //MailMessage.SendMail("Podanie o pracę", "Podanie o pracę zostało poprawnie przesłane i przyjęte.", p.Email);
                //--------------------------------------------------------------------------------

                return RedirectToAction(nameof(JobAplicationConfirmation),new { message = "Pomyślnie przesłano formularz" });
            }
            return RedirectToAction(nameof(JobAplicationIndex));
        }
    }
}
