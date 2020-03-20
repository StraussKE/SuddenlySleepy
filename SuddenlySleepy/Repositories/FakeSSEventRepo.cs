using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuddenlySleepy.Models;

namespace SuddenlySleepy.Repositories
{
    public class FakeSSEventRepo : ISSEventRepo
    {
        private List<SSEvent> ssEvents = new List<SSEvent>();
        public IQueryable<SSEvent> SSEvents => ssEvents.AsQueryable();

        public void AddSSEvent(SSEvent entry)
        {
            ssEvents.Add(entry);
        }
    }
}
