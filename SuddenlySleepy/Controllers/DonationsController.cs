using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuddenlySleepy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SuddenlySleepy.Controllers;

namespace SuddenlySleepy
{
    [Authorize] // only admin can access admin stuff
    public class DonationsController : Controller
    {
        private readonly AppIdentityDbContext _context;
        private UserManager<SSUser> _userManager;

        public DonationsController(AppIdentityDbContext context, UserManager<SSUser> usrMgr)
        {
            _context = context;
            _userManager = usrMgr;
        }

        

        
    }
}
