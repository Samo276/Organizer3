using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organizer3.Areas.Identity.Data;
using Organizer3.Data;
using Organizer3.Models;
using Organizer3.Models.EmployeeEditor;

namespace Organizer3.Controllers
{
    public class UserViewerController : Controller
    {
        private readonly OrganizerDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public UserViewerController(OrganizerDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }



        // GET: UserViewerController
        public async Task<ActionResult> UserViewerIndex()
        {
            if(await IsUserBlockedFromAccesingUserViewer())              
                return  RedirectToAction(nameof(Index), "Home");

            return View(await GetEmployees(true));
        }
        public async Task<ActionResult> LegacyUserViewerIndex()
        {
            if (await IsUserBlockedFromAccesingUserViewer())
                return RedirectToAction(nameof(Index), "Home");

            return View(await GetEmployees(false));

        }
        public async Task<List<UserViewerModel>> GetEmployees(bool employedOnes)
        {
            var userList = new List<UserViewerModel>();
            var allEmployees = await _context.EmploymentStatuses.Where(x => x.IsEmployed == employedOnes).ToListAsync();
            if (allEmployees.Any())
            {
                foreach (var employee in allEmployees)
                {
                    var getUser = await _context.Users.FirstAsync(x => x.Id == employee.UserId);
                    var getFacility = await _context.Facilities.FirstAsync(x => x.Id == employee.FacilityId);
                    userList.Add(new UserViewerModel(
                       employee.UserId,
                       getUser.FirstName,
                       getUser.LastName,
                       employee.Ocupation,
                       getFacility.Name
                       ));
                }
                return userList;
            }
            return userList;
        }
        // GET: UserViewerController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (await IsUserBlockedFromAccesingUserViewer())
                return RedirectToAction(nameof(Index), "Home");

            var getUser = await _context.Users.FirstAsync(x => x.Id == id);
            var getEmploymentInfo = await _context.EmploymentStatuses.FirstAsync(x => x.UserId == id);
            var getFacility = await _context.Facilities.FirstAsync(x => x.Id == getEmploymentInfo.FacilityId);
            var getUserAccess = await _context.AccessPermisions.FirstAsync(x => x.UserId == id);
            var toView = new UserEditModel(getUser, getUserAccess, getEmploymentInfo, getFacility);

            return View();
        }
        public async Task<ActionResult> EmploymentStatusEditor(string id)
        {
            if (await IsUserBlockedFromAccesingUserViewer())
                return RedirectToAction(nameof(Index), "Home");

            var userName = await _context.Users.FirstAsync(u => u.Id == id);
            ViewBag.name = userName.LastName + " " + userName.FirstName;

            var toView = ConvertEmploymentStatusModel(id);
            return View(toView);
        }
        public EmploymentStatusModel ConvertEmploymentStatusModel(string id)
        {
            var getUserdata = _context.EmploymentStatuses.First(x => x.UserId == id);
            return new EmploymentStatusModel
            {
                Id = getUserdata.Id,
                UserId = getUserdata.UserId,
                IsEmployed = getUserdata.IsEmployed,
                Ocupation = getUserdata.Ocupation,
                EmployedSince = getUserdata.EmployedSince,
                ContractType = getUserdata.ContractType,
                ContractExpiration = getUserdata.ContractExpiration,
                SupervisorId = getUserdata.SupervisorId,
                otherInfo = getUserdata.otherInfo
            };
        }
        // POST: UserViewerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EmploymentStatusEditor(string id, EmploymentStatusModel es)
        {
            if (await IsUserBlockedFromAccesingUserViewer())
                return RedirectToAction(nameof(Index), "Home");

            //TODO - dodac edycje placowki
            if (ModelState.IsValid)
                try
                {
                    var tmp = await _context.EmploymentStatuses.FirstAsync(x => x.Id == es.Id);
                    tmp.Ocupation = es.Ocupation;
                    tmp.IsEmployed = es.IsEmployed;
                    tmp.EmployedSince = es.EmployedSince;
                    tmp.ContractType = es.ContractType;
                    tmp.ContractExpiration = es.ContractExpiration;
                    tmp.otherInfo = es.otherInfo;
                    _context.Update(tmp);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("EmployeeProfileIndex", "EmployeeProfile",new { employeeId = es.UserId });
                }
                catch
                {

                }

            return RedirectToAction(nameof(UserViewerIndex));


        }

        public async Task<ActionResult> UserDataEditor(string id)
        {
            if (await IsUserBlockedFromAccesingUserViewer())
                return RedirectToAction(nameof(Index), "Home");

            var tmp = await _context.Users.FirstAsync(x => x.Id == id);
            var toView = new UserDataEditModel(tmp);

            return View(toView);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserDataEditor(string id, UserDataEditModel fromView)
        {
            if (await IsUserBlockedFromAccesingUserViewer())
                return RedirectToAction(nameof(Index), "Home");

            if (ModelState.IsValid)
            {
                var tmp = await _context.Users.FirstAsync(x => x.Id == id);

                tmp.FirstName = fromView.FirstName;
                tmp.LastName = fromView.LastName;
                tmp.SecondaryName = fromView.SecondaryName;
                tmp.LastName = fromView.LastName;
                tmp.Email = fromView.Email;
                tmp.PhoneNumber = fromView.PhoneNo;
                tmp.City = fromView.City;
                tmp.PostalCode = fromView.PostalCode;
                tmp.Street = fromView.Street;
                tmp.ApartmentNumber = fromView.ApartmentNumber;

                _context.Users.Update(tmp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(UserViewerIndex));
            }

            return View(fromView);
        }
        public async Task<ActionResult> UserAccessDataEditor(string id)
        {
            if (await IsUserBlockedFromAccesingUserViewer())
                return RedirectToAction(nameof(Index), "Home");

            var tmp = await _context.AccessPermisions.FirstAsync(x => x.UserId == id);
            var getUserName = await _context.Users.Where(x => x.Id == id).FirstAsync();
            ViewBag.UserName = getUserName.FirstName + " " + getUserName.LastName;
            var toView = new UserAccessDataEditModel
            {
                Id = tmp.Id,
                UserId = tmp.UserId,
                Recruter = tmp.Recruter,
                Scheduler = tmp.Scheduler,
                LeaveEditor = tmp.LeaveEditor,
                UserEditor = tmp.UserEditor,
                UserViewer = tmp.UserViewer,
                FacilityEditor = tmp.FacilityEditor,
                FacilityViewer = tmp.FacilityViewer,
                Announcer = tmp.Announcer,
                PersonalViewer = tmp.PersonalViewer,
                PartnerViewer = tmp.PartnerViewer
            };

            return View(toView); //TODO Zmienić nazwy elementów na nazwy fucnji aplikacji
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserAccessDataEditorApply(string id, UserAccessDataEditModel fromView)
        {
            if (await IsUserBlockedFromAccesingUserViewer())
                return RedirectToAction(nameof(Index), "Home");

            if (ModelState.IsValid)
            {
                var tmp = await _context.AccessPermisions.FirstAsync(x => x.UserId == fromView.UserId);

                tmp.Recruter = fromView.Recruter;
                tmp.Scheduler = fromView.Scheduler;
                tmp.LeaveEditor = fromView.LeaveEditor;
                tmp.UserEditor = fromView.UserEditor;
                tmp.UserViewer = fromView.UserViewer;
                tmp.FacilityEditor = fromView.FacilityEditor;
                tmp.FacilityViewer = fromView.FacilityViewer;
                tmp.Announcer = fromView.Announcer;
                tmp.PersonalViewer = fromView.PersonalViewer;
                tmp.PartnerViewer = fromView.PartnerViewer;


                _context.AccessPermisions.Update(tmp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(UserViewerIndex));
            }

            return View(fromView);
        }
        public async Task<ActionResult> EditUserPhoto(string id)
        {
            if (await IsUserBlockedFromAccesingUserViewer())
                return RedirectToAction(nameof(Index), "Home");

            var tmp = await _context.Users.FirstAsync(x => x.Id == id);

            var toView = new EditUrserPhotoModel
            {
                Id = tmp.Id,
                PhotoLocation = tmp.PhotoLocation,
                Name = tmp.FirstName + " " + tmp.LastName
            };
            return View(toView);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUserPhoto(EditUrserPhotoModel p)
        {
            if (await IsUserBlockedFromAccesingUserViewer())
                return RedirectToAction(nameof(Index), "Home");

                if (ModelState.IsValid)
                {
                    string fileName = Path.GetFileName(p.Photo.FileName);
                    string path = Path.Combine(_environment.WebRootPath, "uploads");
                    Guid guid = Guid.NewGuid();
                    string FileDirectory = Path.Combine(path, guid.ToString());
                    string OriginalFilePath = Path.Combine(path, guid.ToString(), fileName);
                    Directory.CreateDirectory(FileDirectory);
                    using (FileStream stream = new(OriginalFilePath, FileMode.Create))
                    {
                        await p.Photo.CopyToAsync(stream);
                    }

                    var tmp = _context.Users.FirstOrDefault(x => x.Id == p.Id);
                    var oldphoto = new FileInfo(_environment.WebRootPath + "/" + tmp.PhotoLocation);
                    if (oldphoto.Exists)
                    {
                        //System.IO.File.Exists((oldphoto.Name))
                        //System.IO.File.Delete(oldphoto.FullName);
                        oldphoto.Delete();
                        //System.IO.File.Exists(oldphoto.DirectoryName);
                        oldphoto.Directory.Delete();

                    }
                    tmp.PhotoLocation = string.Concat("/uploads/", guid.ToString(), "/" + fileName);
                    //tmp.PhotoLocation = string.Concat("/uploads/", + fileName);
                    _context.Users.Update(tmp);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(UserViewerIndex));
                } 
            
            return RedirectToAction("EditUserPhoto", new {id=p.Id});
        }
        // GET: UserViewerController/Create
        public async Task<ActionResult> CreateNewUser(CreateUserModel fromView)
        {
            if (await IsUserBlockedFromAccesingUserViewer())
                return RedirectToAction(nameof(Index), "Home");

            if (fromView == null)
                return View(new CreateUserModel());
            return View(fromView);
        }

        // POST: UserViewerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateUserModel fromView)
        {
            if (await IsUserBlockedFromAccesingUserViewer())
                return RedirectToAction(nameof(Index), "Home");

            if (ModelState.IsValid)
            {
                var nUser = new AppUser
                {
                    UserName = fromView.Email,
                    FirstName = fromView.FirstName,
                    SecondaryName = fromView.SecondaryName,
                    LastName = fromView.LastName,
                    Email = fromView.Email,
                    City = fromView.City,
                    PostalCode = fromView.PostalCode,
                    Street = fromView.Street,
                    ApartmentNumber = fromView.ApartmentNumber                   
                };
                
                var _password = Guid.NewGuid().ToString().Substring(0, 8); //TODO - ulepszyc generowanie hasel
                string[] randomChars = new[] {
                    "ABCDEFGHJKLMNOPQRSTUVWXYZ",   
                    "!@$?_-" };
                Random rng = new Random();
                _password += "!F";

                //var result = await _userManager.CreateAsync(nUser, "Qwerty`1");
                var result = await _userManager.CreateAsync(nUser, _password);
                if (result.Succeeded)
                {

                    ViewBag.Password = _password;
                    ViewBag.Id = nUser.Id;
                    ViewBag.Email = nUser.Email;

                    try {
                        var tmp = _context.Facilities.First(x => x.Name == "nieprzypisani").Id;
                    } catch {
                        _context.Facilities.Add(new Facility
                        {
                            Name = "nieprzypisani",
                            Region = String.Empty,
                            City = String.Empty,
                            PostalCode = String.Empty,
                            Adress = String.Empty,
                            PhoneNo = String.Empty,
                            AdditionalInfo = String.Empty,
                        });
                        await _context.SaveChangesAsync();
                    }

                    var newEmploymentstatus = new EmploymentStatus
                    {
                        UserId = nUser.Id,
                        IsEmployed = true,
                        Ocupation = fromView.Ocupation,
                        EmployedSince = fromView.EmployedSince,
                        ContractType = fromView.ContractType,
                        ContractExpiration = fromView.ContractExpiration,
                        otherInfo = fromView.otherInfo,
                        FacilityId = (_context.Facilities.First(x => x.Name == "nieprzypisani").Id)
                    };

                    var newUserAccessPemissions = new UserAccess
                    {
                        UserId = nUser.Id,
                        Recruter = fromView.Recruter,
                        Scheduler = fromView.Scheduler,
                        LeaveEditor = fromView.LeaveEditor,
                        UserEditor = fromView.UserEditor,
                        UserViewer = fromView.UserViewer,
                        FacilityEditor = fromView.FacilityEditor,
                        FacilityViewer = fromView.FacilityViewer,
                        Announcer = fromView.Announcer,
                        PersonalViewer = fromView.PersonalViewer,
                        PartnerViewer = fromView.PartnerViewer
                    };

                    nUser.Accesses=newUserAccessPemissions;
                    nUser.EmploymentStatus = newEmploymentstatus;
                    
                    //tmp.Accesses = newUserAccessPemissions;
                    //tmp.EmploymentStatus = newEmploymentstatus;
                    await _context.SaveChangesAsync();
                    _context.Users.First(y=>y.Id==nUser.Id).EmailConfirmed = true;
                    await _context.SaveChangesAsync();
                    return RedirectToAction("AfterUserCreation",new AfterUserCreationModel {password=_password,login=nUser.Email,Id=nUser.Id,name=nUser.LastName+" "+nUser.FirstName });
                }
                //var tmp = result.Errors;
            }
            return RedirectToAction(nameof(CreateNewUser), fromView);
        }
        public async Task<ActionResult> AfterUserCreation(AfterUserCreationModel toView)
        {
            if (await IsUserBlockedFromAccesingUserViewer())
                return RedirectToAction(nameof(Index), "Home");

            return View(toView);
        }
        /// <summary>
        /// Checks if user is logged in and is alowed to access the content of the UserViewer section, 
        /// returns FALSE if user is alowed in, and TRUE when access is forbidden
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IsUserBlockedFromAccesingUserViewer()
        {
            if (User.Identity.IsAuthenticated)
            {
                var tmp = await _context.AccessPermisions.FirstAsync(u=>u.UserId==_userManager.GetUserId(User));
                if(tmp.UserViewer != null)
                    if (tmp.UserViewer)
                        return false;               
            }
            return true;
        }
    }
}
