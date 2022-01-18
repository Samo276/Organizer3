using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organizer3.Areas.Identity.Data;
using Organizer3.Data;
using Organizer3.Models;

namespace Organizer3.Controllers
{
    public class UserViewerController : Controller
    {
        private readonly OrganizerDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public UserViewerController(OrganizerDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: UserViewerController
        public async Task<ActionResult> UserViewerIndex()
        {
            if (User.Identity.IsAuthenticated)
                return View(await GetEmployees(true));
            return View(new List<UserViewerModel>());
        }
        public async Task<ActionResult> LegacyUserViewerIndex()
        {
            if (User.Identity.IsAuthenticated)
                return View(await GetEmployees(false));
            return View(new List<UserViewerModel>());
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
            var getUser = await _context.Users.FirstAsync(x => x.Id == id);
            var getEmploymentInfo = await _context.EmploymentStatuses.FirstAsync(x => x.UserId == id);
            var getFacility = await _context.Facilities.FirstAsync(x => x.Id == getEmploymentInfo.FacilityId);
            var getUserAccess = await _context.AccessPermisions.FirstAsync(x => x.UserId == id);
            var toView = new UserEditModel(getUser, getUserAccess, getEmploymentInfo, getFacility);
            

            return View();
        }
        public async Task<ActionResult> EmploymentStatusEditor(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var toView = await _context.EmploymentStatuses.FirstAsync(x => x.UserId == id);
                var userName = await _context.Users.FirstAsync(u => u.Id == id);
                ViewBag.name =userName.LastName + " "+userName.FirstName;
                //ViewBag.faclilities = new FacilityIdAndNameStorage(
                  //  await _context.Facilities.FirstAsync()
                return View(toView);
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: UserViewerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EmploymentStatusEditor(string id,EmploymentStatus es)
        {
            //TODO - check model validation nie działa
            //if (ModelState.IsValid)
            try
            {
                var tmp = await _context.EmploymentStatuses.FirstAsync(x => x.Id == es.Id);
                tmp.IsEmployed = es.IsEmployed;
                tmp.EmployedSince = es.EmployedSince;
                tmp.ContractType = es.ContractType;
                tmp.ContractExpiration = es.ContractExpiration;
                tmp.otherInfo = es.otherInfo;
                _context.Update(tmp);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(UserViewerIndex));
            }
            catch
            {

            }
            return View(id);
            
            
        }

        // GET: UserViewerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserViewerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserViewerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserViewerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserViewerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserViewerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
