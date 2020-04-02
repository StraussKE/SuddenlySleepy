using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuddenlySleepy.Models;
using Microsoft.EntityFrameworkCore;


namespace SuddenlySleepy.Repositories
{
    public class EFSSEventRepo : ISSEventRepo
    {
        private AppIdentityDbContext context;

        public EFSSEventRepo(AppIdentityDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<SSEvent> SSEvents => context.SSEvents.Include(sse => sse.RegisteredAttendees);


        public void AddSSEvent(SSEvent ssEvent)
        {
            context.Add(ssEvent);
            context.SaveChanges();
        }
    }
}
