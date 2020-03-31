using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuddenlySleepy.Models
{
    public class Donation
    {
        public Guid DonationId {get;set;}
        public double DonationAmount { get; set; }
        public DateTime DonationDate { get; set; }
        public virtual SSUser Donor { get; set; }
    }
}
