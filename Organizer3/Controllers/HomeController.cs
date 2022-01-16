﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Organizer3.Areas.Identity.Data;
using Organizer3.Data;
using Organizer3.Models;
using System.Diagnostics;

namespace Organizer3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly OrganizerDbContext  _context;
        private readonly UserManager<AppUser>  _userManager;



        public HomeController(OrganizerDbContext context, UserManager<AppUser> userManager, ILogger<HomeController> logger)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            var cuid = _userManager.GetUserId(User);
            var cu = new TestModel(_context.AccessPermisions.First(x => x.UserId == cuid), cuid);
            return View(cu);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}