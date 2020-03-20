using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuddenlySleepy.Models;

namespace SuddenlySleepy.Repositories
{
    public class EFDonationRepo : IDonationRepo
    {
        private AppIdentityDbContext context;

        public EFDonationRepo(AppIdentityDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Donation> Donations => context.Donations;


        public void AddDonation(Donation donation)
        {
            context.Add(donation);
            context.SaveChanges();
        }
    }
}
