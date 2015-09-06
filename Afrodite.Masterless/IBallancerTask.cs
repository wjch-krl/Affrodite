using System.Collections.Generic;
using System;

namespace AffroditeP2P
{
    public interface IBallancerTask<TJob,TJobType>
    {
		IEnumerable<TJob> GetJobs(TJobType priority);
		IDictionary<TJobType,Tuple<int, int>> JobWorkloads{ get; }
        bool StartJob(TJob job);
    }
}
