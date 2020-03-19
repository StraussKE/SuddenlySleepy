using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuddenlySleepy.Models
{
    public class SSUserSSEvent
    {
        public string SSUserId { get; set; }
        public SSUser SSUser { get; set; }

        public Guid SSEventId { get; set; }
        public SSEvent SSEvent { get; set; }
    }
}
