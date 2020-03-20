using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuddenlySleepy.Models;

namespace SuddenlySleepy.Repositories
{
    interface IDonationRepo
    {
        IQueryable<Donation> Donations { get; }

        void AddDonation(Donation donation);
    }
}
