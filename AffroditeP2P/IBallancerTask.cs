using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Afrodite.Abstract;

namespace AffroditeP2P
{
    public interface IBallancerTask<TJob>
    {
        IEnumerable<TJob> GetJobs(int priority);
        IJob<TJob> StartJob(TJob job);
    }
}
