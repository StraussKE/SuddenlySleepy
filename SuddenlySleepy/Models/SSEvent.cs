using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SuddenlySleepy.Models
{
    public class SSEvent
    {
        [Key]
        public Guid SSEventId { get; set; }
        public DateTime MeetingDate { get; set; }
        public string Location { get; set; }
        public string SSEventName { get; set; }
        public string Description { get; set; }

        public virtual List<SSUserSSEvent> RegisteredAttendees { get; set; }

    }
}
