using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuddenlySleepy.Models
{
    public class SSUserSSEvent
    {
        public string sSUserId { get; set; }
        public SSUser sSUser { get; set; }

        public Guid sSEventId { get; set; }
        public SSEvent sSEvent { get; set; }
    }
}
