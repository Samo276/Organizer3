using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Organizer3.Areas.Identity.Data;
using Organizer3.Data;
using Organizer3.Models.Enums;
using System.Text.Json;
namespace Organizer3.Controllers
{
    public class Recruiter : Controller
    {
        private readonly OrganizerDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public Recruiter(OrganizerDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        public async Task<IActionResult> RecruiterIndex()
        {
            #region FillDatabseWithSampleEntities
            /*var x = new List<Recruitment>
            {
                new Recruitment
                {
                 //   id = 1,
                    FirstName ="aaaaa",
                    LastName =  "bbbbbb",
                    Email ="llllllllll",
                    PhoneNumber="23472364",
                    ResumeLocation="D:\\test\\podanie.pdf",
                    Position="seller",
                    Status =RecruterEnum.RecruterEnumData.Applied.ToString(),
                    AppliedAt =DateTime.Now.AddDays(-9),
                    Notes="aaaaaa"
                },
                new Recruitment
                {
                 //   id = 7,
                    FirstName ="aaaaa",
                    LastName =  "bbbbbb",
                    Email ="llllllllll",
                    PhoneNumber="23472364",
                    ResumeLocation="D:\\test\\podanie.pdf",
                    Position="seller",
                    Status =RecruterEnum.RecruterEnumData.Accepted.ToString(),
                    AppliedAt =DateTime.Now.AddDays(-9),
                    Notes="aaaaaa"
                },
                new Recruitment
                {
                 //   id = 8,
                    FirstName ="aaaaa",
                    LastName =  "bbbbbb",
                    Email ="llllllllll",
                    PhoneNumber="23472364",
                    ResumeLocation="D:\\test\\podanie.pdf",
                    Position="seller",
                    Status =RecruterEnum.RecruterEnumData.Rejected.ToString(),
                    AppliedAt =DateTime.Now.AddDays(-9),
                    Notes="aaaaaa"
                },
                new Recruitment
                {
                 //   id = 5,
                    FirstName ="aaaaa",
                    LastName =  "bbbbbb",
                    Email ="llllllllll",
                    PhoneNumber="23472364",
                    ResumeLocation="D:\\test\\podanie.pdf",
                    Position="seller",
                    Status =RecruterEnum.RecruterEnumData.InRecruitment.ToString(),
                    AppliedAt =DateTime.Now.AddDays(-9),
                    Notes="aaaaaa"
                },
                new Recruitment
                {
                 //   id = 6,
                    FirstName ="aaaaa",
                    LastName =  "bbbbbb",
                    Email ="llllllllll",
                    PhoneNumber="23472364",
                    ResumeLocation="D:\\test\\podanie.pdf",
                    Position="seller",
                    Status =RecruterEnum.RecruterEnumData.InRecruitment.ToString(),
                    AppliedAt =DateTime.Now.AddDays(-9),
                    Notes="aaaaaa"
                },
                new Recruitment
                {
                 //   id = 2,
                    FirstName ="aaaaa",
                    LastName =  "bbbbbb",
                    Email ="llllllllll",
                    PhoneNumber="23472364",
                    ResumeLocation="D:\\test\\podanie.pdf",
                    Position="seller",
                    Status =RecruterEnum.RecruterEnumData.Applied.ToString(),
                    AppliedAt =DateTime.Now.AddDays(-9),
                    Notes="aaaaaa"
                },
                new Recruitment
                {
                 //   id = 3,
                    FirstName ="aaaaa",
                    LastName =  "bbbbbb",
                    Email ="llllllllll",
                    PhoneNumber="23472364",
                    ResumeLocation="D:\\test\\podanie.pdf",
                    Position="seller",
                    Status =RecruterEnum.RecruterEnumData.Applied.ToString(),
                    AppliedAt =DateTime.Now,
                    Notes="aaaaaa"
                },
                new Recruitment
                {
                 //   id = 4,
                    FirstName ="aaaaa",
                    LastName =  "bbbbbb",
                    Email ="llllllllll",
                    PhoneNumber="23472364",
                    ResumeLocation="D:\\test\\podanie.pdf",
                    Position="seller",
                    Status =RecruterEnum.RecruterEnumData.Applied.ToString(),
                    AppliedAt =DateTime.Now.AddDays(29),
                    Notes="aaaaaa"
                },
            };
            foreach (var item in x)
            {
                _context.Recruitments.Add(item);
            }
            await _context.SaveChangesAsync();*/
            #endregion

            return View(_context.Recruitments.OrderBy(x => x.AppliedAt).ToList());
        }
        public async Task<IActionResult> DisplayFistStage(int Id)
        {
            ViewBag.tmp = File(@"D:\Ath\Organizer3\Organizer3\wwwroot\uploads\podanie.pdf", "application/pdf");

            Recruitment toView = _context.Recruitments.First(x => x.id == Id);
            return View(toView);
        }
        public async Task<IActionResult> Reject(int Id)
        {
            var tmp = _context.Recruitments.First(x => x.id == Id);
            tmp.Status = RecruterEnum.RecruterEnumData.Rejected.ToString();
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(RecruiterIndex));
        }
        public async Task<IActionResult> AddToRecruitmentInProcess(int Id)
        {
            var tmp = _context.Recruitments.First(x => x.id == Id);
            tmp.Status = RecruterEnum.RecruterEnumData.InRecruitment.ToString();
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(RecruiterIndex));
        }
        public async Task<IActionResult> AddToAccepted(int Id)
        {
            var tmp = _context.Recruitments.First(x => x.id == Id);
            tmp.Status = RecruterEnum.RecruterEnumData.InRecruitment.ToString();
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(RecruiterIndex));
        }

        public async Task<IActionResult> DisplayInRecruitment(int Id)
        {
            /*var x = new List<string>();
            x.Add("notatka 1");
            x.Add("notatka 2");
            var jj = JsonSerializer.Serialize(x);

            foreach (var item in _context.Recruitments.ToList())
            {
                item.Notes = jj;
            }*/
            await _context.SaveChangesAsync();
            var tmp = _context.Recruitments.First(x => x.id == Id);
            if (!(tmp.Notes == null || tmp.Notes == string.Empty))
            {
                List<string> notes = JsonSerializer.Deserialize<List<string>>(tmp.Notes);
                ViewBag.notes = notes;
            }
            else
            {
                ViewBag.notes = new List<string> { "Na razie brak notatek" };
            }
            return View(tmp);
        }

        public async Task<IActionResult> AddNote(int Id)
        {
            return View();
        }
        
        public async Task<IActionResult> AddNoteToDatabase(int Id)
        {
            if (ModelState.IsValid)
            {
                var tmp = _context.Recruitments.First(x => x.id == Id);
                if (!(tmp.Notes == null || tmp.Notes == string.Empty))
                {
                    List<string> notes = JsonSerializer.Deserialize<List<string>>(tmp.Notes);
                }
                else
                {
                    //tmp.Notes = JsonSerializer.Serialize<List<string>>(new List<string> { ModelState.Values["id"] });
                }
                //TODO - finish this view
            }
            return RedirectToAction(nameof(AddNote),Id);
        }
    }
}
