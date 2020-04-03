﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SuddenlySleepy.Models;

namespace SuddenlySleepy.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppIdentityDbContext _context;


        public HomeController(AppIdentityDbContext ctx)
        {
            _context = ctx;
        }

        public async Task<IActionResult> Index()
        {
            var nextEvent = _context.SSEvents.OrderBy(e => e.MeetingDate).Where(e => e.MeetingDate > DateTime.Now).First();
            return View(nextEvent);
        }

        public async Task<IActionResult> About()
        {
            return View();
        }

        public IActionResult AccessDenied()
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
