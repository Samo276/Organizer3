using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organizer3.Areas.Identity.Data;
using Organizer3.Data;
using Organizer3.Models.Enums;
using Organizer3.Models.Recriter;
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
        public FileStreamResult CV_pdf(string path)
        {
            FileStream fs = new FileStream(_environment.WebRootPath+path, FileMode.Open, FileAccess.Read);
            return File(fs, "application/pdf");
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

            if(await IsUserBlockedFromAccesingRecruiter())
                return RedirectToAction(nameof(Index), "Home");

            return View(_context.Recruitments.OrderByDescending(x => x.AppliedAt).ToList());
        }
        public async Task<IActionResult> DisplayFistStage(int Id)
        {
            if (await IsUserBlockedFromAccesingRecruiter())
                return RedirectToAction(nameof(Index), "Home");

            ViewBag.tmp = File(@"D:\Ath\Organizer3\Organizer3\wwwroot\uploads\podanie.pdf", "application/pdf");

            Recruitment toView = _context.Recruitments.First(x => x.id == Id);
            return View(toView);
        }
        public async Task<IActionResult> Reject(int Id)
        {
            if (await IsUserBlockedFromAccesingRecruiter())
                return RedirectToAction(nameof(Index), "Home");

            var tmp = _context.Recruitments.First(x => x.id == Id);
            tmp.Status = RecruterEnum.RecruterEnumData.Rejected.ToString();

            _context.recruitmentNotes.Add(Create_System_RecruitmentNote(Id, "Odrzuca Podanie o pracę i  archiwizuje."));
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(RecruiterIndex));
        }
        public async Task<IActionResult> AddToRecruitmentInProcess(int Id)
        {
            if (await IsUserBlockedFromAccesingRecruiter())
                return RedirectToAction(nameof(Index), "Home");

            var tmp = _context.Recruitments.First(x => x.id == Id);
            tmp.Status = RecruterEnum.RecruterEnumData.InRecruitment.ToString();

            _context.recruitmentNotes.Add(Create_System_RecruitmentNote(Id, "Dopuszcza do rekrutacji"));

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(RecruiterIndex));
        }
        public async Task<IActionResult> AddToAccepted(int Id)
        {
            if (await IsUserBlockedFromAccesingRecruiter())
                return RedirectToAction(nameof(Index), "Home");

            var tmp = _context.Recruitments.First(x => x.id == Id);
            tmp.Status = RecruterEnum.RecruterEnumData.Accepted.ToString();

            _context.recruitmentNotes.Add(Create_System_RecruitmentNote(Id, "Wstępnie zaakceptowano jako pracownika"));

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(RecruiterIndex));
        }
        
        public async Task<IActionResult> AddToArchived(int Id)
        {
            if (await IsUserBlockedFromAccesingRecruiter())
                return RedirectToAction(nameof(Index), "Home");

            var tmp = _context.Recruitments.First(x => x.id == Id);
            tmp.Status = RecruterEnum.RecruterEnumData.Archived.ToString();

            _context.recruitmentNotes.Add(Create_System_RecruitmentNote(Id, "Przeniesiono do Archiwum"));

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(RecruiterIndex));
        }

        public async Task<IActionResult> DisplayInRecruitment(int Id)
        {
            if (await IsUserBlockedFromAccesingRecruiter())
                return RedirectToAction(nameof(Index), "Home");

            await _context.SaveChangesAsync();
            var tmp = _context.Recruitments.First(x => x.id == Id);
            tmp.Recruit_Notes = _context.recruitmentNotes.Where(x=>x.RecruitmentId == Id).OrderByDescending(y => y.CreatedDate).ToList();
            
            if(tmp.Recruit_Notes ==null)
                tmp.Recruit_Notes = new List<RecruitmentNotes>();
            
            if (tmp.Notes==null)
                tmp.Notes = String.Empty;

            return View(tmp);
        }
        
        public async Task<IActionResult> DeleteRecritmentFromArchiveConfirmation(int Id, string c_name)
        {
            if (await IsUserBlockedFromAccesingRecruiter())
                return RedirectToAction(nameof(Index), "Home");

            return View(
                new DeleteRecritmentFromArchiveConfirmationModel{
                    Id = Id,
                    Name = c_name
                }
            );
        }
        
        public async Task<IActionResult> DeleteEntityFromRecruitementArchive(int Id)
        {
            if (await IsUserBlockedFromAccesingRecruiter())
                return RedirectToAction(nameof(Index), "Home");

            var tmp = _context.Recruitments.First(x => x.id == Id);
                var oldphoto = new FileInfo(_environment.WebRootPath + tmp.ResumeLocation);
                if (oldphoto.Exists)
                {
                    oldphoto.Delete();
                    oldphoto.Directory.Delete();

                }
                
            _context.Recruitments.Remove(tmp);
            var toRemove =  _context.recruitmentNotes.Where(x => x.RecruitmentId == Id).ToList();
            foreach (var item in toRemove)
            {
                _context.recruitmentNotes.Remove(item);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(RecruiterIndex));
        }
        public async Task<IActionResult> AddNote(int Id)
        {
            if (await IsUserBlockedFromAccesingRecruiter())
                return RedirectToAction(nameof(Index), "Home");

            var tmp = new Models.Recriter.AddRecruitmentNoteModel();
            var entityTmp = _context.Recruitments.First(x=>x.id == Id);
            tmp.Aplicant = entityTmp.LastName+" "+entityTmp.FirstName;
            tmp.Id = Id; 
            return View(tmp);
        }
        public async Task<IActionResult> AddNoteToDatabase(Models.Recriter.AddRecruitmentNoteModel model)
        {
            if (await IsUserBlockedFromAccesingRecruiter())
                return RedirectToAction(nameof(Index), "Home");

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
        private RecruitmentNotes Create_System_RecruitmentNote(int id, string noteContent)
        {            
            var author_name_tmp = _context.Users.First(x => x.Id == _userManager.GetUserId(User));
            return new RecruitmentNotes
                {
                    CreatedDate = DateTime.Now,
                    RecruitmentId = id,
                    NoteAuthor = author_name_tmp.LastName + " " + author_name_tmp.FirstName,
                    NoteContent = noteContent
                };           
        }
        /// <summary>
        /// Checks if user is logged in and is alowed to access the content of the Recruiter section, 
        /// returns FALSE if user is alowed in, and TRUE when access is forbidden
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IsUserBlockedFromAccesingRecruiter()
        {
            if (User.Identity.IsAuthenticated)
            {
                var tmp = await _context.AccessPermisions.FirstAsync(u => u.UserId == _userManager.GetUserId(User));
                if (tmp.Recruter != null)
                    if (tmp.Recruter)
                        return false;
            }
            return true;
        }
    }
}
