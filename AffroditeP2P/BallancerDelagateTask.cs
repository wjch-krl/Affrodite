using System;
using System.Collections.Generic;

namespace AffroditeP2P
{
    class BallancerDelagateTask<TJob> : IBallancerTask<TJob>
    {
        private readonly Func<int, IEnumerable<TJob>> getJobsFunc;
        private readonly Func<TJob, bool> startTaskFunc;

        public BallancerDelagateTask(Func<int, IEnumerable<TJob>> getJobsFunc, Func<TJob, bool> startTaskFunc, int maxPrior)
        {
            this.getJobsFunc = getJobsFunc;
            this.startTaskFunc = startTaskFunc;
            this.MaxPriority = maxPrior;
        }

        public IEnumerable<TJob> GetJobs(int priority)
        {
            return getJobsFunc(priority);
        }

        public bool StartJob(TJob job)
        {
            return startTaskFunc(job);
        }

        public int MaxPriority { get; private set; }
    }
}