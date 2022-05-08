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
            tmp.Status = RecruterEnum.RecruterEnumData.Accepted.ToString();
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(RecruiterIndex));
        }

        public async Task<IActionResult> DisplayInRecruitment(int Id)
        {

            await _context.SaveChangesAsync();
            var tmp = _context.Recruitments.First(x => x.id == Id);
            tmp.Recruit_Notes = _context.recruitmentNotes.Where(x=>x.RecruitmentId == Id).OrderByDescending(y => y.CreatedDate).ToList();
            
            if(tmp.Recruit_Notes ==null)
                tmp.Recruit_Notes = new List<RecruitmentNotes>();
            
            if (tmp.Notes==null)
                tmp.Notes = String.Empty;

            return View(tmp);
        }

        public async Task<IActionResult> AddNote(int Id)
        {
            var tmp = new Models.Recriter.AddRecruitmentNoteModel();
            var entityTmp = _context.Recruitments.First(x=>x.id == Id);
            tmp.Aplicant = entityTmp.LastName+" "+entityTmp.FirstName;
            tmp.Id = Id; 
            return View(tmp);
        }
        public async Task<IActionResult> AddNoteToDatabase(Models.Recriter.AddRecruitmentNoteModel model)
        {
            var tmp_id = model.Id;
            if (ModelState.IsValid)
            {
                if (_context.Recruitments.Any(x => x.id == model.Id)){
                    var tmp = new RecruitmentNotes();
                    tmp.RecruitmentId = model.Id;
                    tmp.CreatedDate = DateTime.Now;
                    tmp.NoteContent = model.NoteContent;

                    var user_tmp = _context.Users.First(x=>x.Id == _userManager.GetUserId(User));
                    tmp.NoteAuthor = user_tmp.LastName+" "+user_tmp.FirstName;
                    _context.recruitmentNotes.Add(tmp);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("DisplayInRecruitment", new { Id = tmp_id });
                }
            }
            return RedirectToAction("AddNote",new { Id = tmp_id });
        }
        
        /*public async Task<IActionResult> AddNoteToDatabase(int Id)
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
        }*/
    }
}
