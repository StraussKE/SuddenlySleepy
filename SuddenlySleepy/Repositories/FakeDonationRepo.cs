using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuddenlySleepy.Models;
using Microsoft.EntityFrameworkCore;

namespace SuddenlySleepy.Repositories
{
    public class FakeDonationRepo : IDonationRepo
    {
        private List<Donation> donation = new List<Donation>();
        public IQueryable<Donation> Donations => donation.AsQueryable(); // not including donor data in this because teasing and identity are not friends

        public void AddDonation(Donation entry)
        {
            donation.Add(entry);
        }
    }
}
