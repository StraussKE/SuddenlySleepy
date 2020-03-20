using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuddenlySleepy.Models;

namespace SuddenlySleepy.Repositories
{
    interface ISSEventRepo
    {
        IQueryable<SSEvent> SSEvents { get; }

        void AddSSEvent(SSEvent entry);
    }
}
