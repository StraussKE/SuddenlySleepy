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

        [Required(ErrorMessage = "Your meeting needs to have a date")]
        public DateTime MeetingDate { get; set; }

        [Required(ErrorMessage = "If unknown, please enter TBD, if online, please provide connection details")]
        public string Location { get; set; }

        [Required(ErrorMessage ="Please provide a brief descriptive name for your event"), MaxLength(75)]
        public string SSEventName { get; set; }

        [Required(ErrorMessage = "Please describe the event in detail")]
        [MaxLength(5000, ErrorMessage = "Your description has a maximum length of 5000 characters"), MinLength(20)]
        public string Description { get; set; }

        public List<SSUserSSEvent> RegisteredAttendees { get; set; } = new List<SSUserSSEvent>();
    }
}
