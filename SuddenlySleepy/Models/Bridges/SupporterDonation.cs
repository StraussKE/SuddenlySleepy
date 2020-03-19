using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuddenlySleepy.Models
{
    public class SSUserDonation
    {
        public string SSUserId { get; set; }
        public SSUser SSUser { get; set; }

        public Guid DonationId { get; set; }
        public Donation Donation { get; set; }
    }
}
