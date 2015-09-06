using System;
using System.Collections.Generic;
using System.Linq;

namespace AffroditeP2P
{
	class BallancerDelagateTask<TJob,TJobType> : IBallancerTask<TJob,TJobType>
	{
		private readonly Func<TJobType, IEnumerable<TJob>> getJobsFunc;
		private readonly Func<TJob, bool> startTaskFunc;

		public BallancerDelagateTask (Func<TJobType, IEnumerable<TJob>> getJobsFunc, Func<TJob, bool> startTaskFunc,
		                             IEnumerable<Tuple<TJobType,Tuple<int,int>>> jobTypesMapping)
		{
			this.JobWorkloads = jobTypesMapping.ToDictionary (y => y.Item1, y => y.Item2);
			this.getJobsFunc = getJobsFunc;
			this.startTaskFunc = startTaskFunc;
		}

		public IEnumerable<TJob> GetJobs (TJobType priority)
		{
			return getJobsFunc (priority);
		}

		public bool StartJob (TJob job)
		{
			return startTaskFunc (job);
		}
			
		public IDictionary<TJobType, Tuple<int, int>> JobWorkloads
		{
			get;
			private set;
		}
	}
}