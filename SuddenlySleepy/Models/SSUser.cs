using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SuddenlySleepy.Models
{
    public class SSUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        // list of Meetings that the user has attended
        public virtual List<SSUserSSEvent> AttendedEvents { get; set; } = new List<SSUserSSEvent>();
    }
}
