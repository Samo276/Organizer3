using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Organizer3.Areas.Identity.Data;
using Organizer3.Data;
using Organizer3.Models.FacilitiesEditor;

namespace Organizer3.Controllers
{
    public class FacilitiesController : Controller
    {
        private readonly OrganizerDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public FacilitiesController(OrganizerDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        public async Task<IActionResult> FacilitiesIndex()
        {
            if(await IsUserBlockedFromAccesingFacilityEditor())
                return RedirectToAction(nameof(Index), "Home");

            return View(
                await ConvertDatabaseEntitiesToModel(
                    await _context.Facilities.ToListAsync()
                    )
                );
        }

        public async Task<IActionResult> Details(int sId)
        {
            if (await IsUserBlockedFromAccesingFacilityEditor())
                return RedirectToAction(nameof(Index), "Home");

            var getEmployeesIds = await _context.EmploymentStatuses.Where(y => y.FacilityId == sId).ToListAsync();
            if (!getEmployeesIds.Any() || getEmployeesIds == null)
                getEmployeesIds = new List<EmploymentStatus>();

            

            var toView = new FacilityProfileModel{
                FacilityData= await ConvertSingleFacilityDatabaseEntitiesToModel(  _context.Facilities.First(y => y.Id == sId) ),
                ShopEmployeeList = await _ConvertToUsercontactInfo(getEmployeesIds),
                HasEmployeeEditorAccess = _context.AccessPermisions.First(y => y.UserId == _userManager.GetUserId(User)).FacilityEditor,
            };
            return View(toView);
        }

        private async Task<List<UserContactInfo>> _ConvertToUsercontactInfo(List<EmploymentStatus> _data)
        {
            var result = new List<UserContactInfo>();

            foreach (var item in _data.Where(y=>y.IsEmployed==true).ToList())
            {
                result.Add(new UserContactInfo
                {
                    UserId = item.UserId,
                    Name = String.Empty,
                    PhoneNo = String.Empty,
                    Position = item.Ocupation
                });
            }
            var tmpUsers = await _context.Users.ToListAsync();

            foreach (var item in result)
            {
                var tmp = tmpUsers.First(y => y.Id == item.UserId);
                if(tmp.SecondaryName != null)
                    item.Name = tmp.LastName + " " + tmp.FirstName+" "+tmp.SecondaryName;
                else
                    item.Name = tmp.LastName+" "+tmp.FirstName;
                item.PhoneNo = tmp.PhoneNumber;
            }
            return result;
        }

        public async Task<IActionResult> Create()
        {
            if (await IsUserBlockedFromAccesingFacilityEditor())
                return RedirectToAction(nameof(Index), "Home");
            
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Info,PhoneNo,PostalCode,Region,City,Adress")] FacilitiesListModel facilitiesListModel)
        {
            if (await IsUserBlockedFromAccesingFacilityEditor())
                return RedirectToAction(nameof(Index), "Home");

            if (ModelState.IsValid)
            {
                var tmp = new Facility
                {
                    City = facilitiesListModel.City,
                    Adress = facilitiesListModel.Adress,
                    Region = facilitiesListModel.Region,
                    PostalCode = facilitiesListModel.PostalCode,
                    PhoneNo = facilitiesListModel.PhoneNo,
                    AdditionalInfo = facilitiesListModel.Info,
                    Name = facilitiesListModel.Name
                };
                _context.Facilities.Add(tmp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(FacilitiesIndex));
            }
            return View(facilitiesListModel);
        }

        // GET: Facilities/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (await IsUserBlockedFromAccesingFacilityEditor())
                return RedirectToAction(nameof(Index), "Home");

            return View(
                await ConvertSingleFacilityDatabaseEntitiesToModel(
                    await _context.Facilities.FindAsync(id)
                    )
                );
        }

        // POST: Facilities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(int? id, FacilitiesListModel fromView)
        {
            if (id == null) return NotFound();
            if (await IsUserBlockedFromAccesingFacilityEditor())
                return RedirectToAction(nameof(Index), "Home");

            if (ModelState.IsValid)
            {

                    var tmp = await _context.Facilities.FirstAsync(y=>y.Id== fromView.Id);
                    tmp.Name = fromView.Name;
                    tmp.Region = fromView.Region;
                    tmp.City = fromView.City;
                    tmp.PostalCode = fromView.PostalCode;
                    tmp.Adress = fromView.Adress;
                    tmp.AdditionalInfo = fromView.Info;
                    tmp.PhoneNo = fromView.PhoneNo;
                    
                    _context.Facilities.Update(tmp);
                    await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Details), new { sId = fromView.Id});
            }
            return View(fromView);
        }
       
        
        /// <summary>
        /// Checks if user is logged in and is alowed to access the content of the Recruiter section, 
        /// returns FALSE if user is alowed in, and TRUE when access is forbidden
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IsUserBlockedFromAccesingFacilityEditor()
        {
            if (User.Identity.IsAuthenticated)
            {
                var tmp = await _context.AccessPermisions.FirstAsync(u => u.UserId == _userManager.GetUserId(User));
                if (tmp.FacilityEditor != null)
                    if (tmp.FacilityEditor)
                        return false;
            }
            return true;
        }
        private async Task<FacilitiesListModel> ConvertSingleFacilityDatabaseEntitiesToModel(Facility _data)
        {
            var result = await ConvertDatabaseEntitiesToModel(new List<Facility> { _data });
            return result.FirstOrDefault();
        }
        private async Task<List<FacilitiesListModel>> ConvertDatabaseEntitiesToModel(List<Facility> _data)
        {
            var tmp = new List<FacilitiesListModel>();
            foreach (var item in _data)
            {
                tmp.Add(new FacilitiesListModel {
                    Id = item.Id,
                    Name = item.Name,
                    Region = item.Region,
                    City = item.City,
                    Adress = item.Adress,
                    PostalCode = item.PostalCode,
                    PhoneNo = item.PhoneNo,
                    Info=item.AdditionalInfo,
                    }
                );
            }
            return tmp;
        }
    }
}
