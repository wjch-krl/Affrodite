using System.Collections.Generic;

namespace AffroditeP2P
{
    public interface IBallancerTask<TJob>
    {
        IEnumerable<TJob> GetJobs(int priority);
        bool StartJob(TJob job);
        int MaxPriority { get; }
    }
}
