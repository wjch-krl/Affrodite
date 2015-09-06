using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Afrodite.Abstract;
using Afrodite.Connection;
using System.Linq;

namespace AffroditeP2P
{
    public class LocalBallancer<TJob,TJobType> : IDisposable
    {
		private readonly IBallancerTask<TJob,TJobType> ballancerTask;
        private IRemoteMachinesManager remoteMachines;
        private readonly int currentMachineId;
        private IPerformanceManager performanceManager;
        private bool run;

		public LocalBallancer(IBallancerTask<TJob,TJobType> ballancerTask, IPerformanceManager performanceManager,
            IRemoteMachinesManager remoteMachines, int currentMachineId)
        {
            if (ballancerTask == null)
                throw new ArgumentNullException("ballancerTask");
            if (performanceManager == null)
                throw new ArgumentNullException("performanceManager");
            if (remoteMachines == null)
                throw new ArgumentNullException("remoteMachines");
            this.ballancerTask = ballancerTask;
            this.performanceManager = performanceManager;
            this.remoteMachines = remoteMachines;
            this.currentMachineId = currentMachineId;
        }

        public void Start()
        {
            run = true;
            do
            {
                var cpuUsage = performanceManager.GetAvgCpusUsage();
				var jobType = CpuUsageToJobType(cpuUsage);
                var toDos = ballancerTask.GetJobs(jobType);
                
                int machinesCount = remoteMachines.Count;
                var unaviableHosts = new HashSet<int>(remoteMachines.UnaviableHosts());
                bool hasStarted = false;
                foreach (var job in toDos)
                {
                    int jobId = job.GetHashCode();
                    if (IsValidForMachine(jobId, machinesCount, currentMachineId, unaviableHosts))
                        //TODO Test random with seed (taskId)
                    {
                        ballancerTask.StartJob(job);
                        hasStarted = true;
                        break;
                    }
                }
                if(!hasStarted)
                {
                    ballancerTask.StartJob(default(TJob));
                }
				//TODO
//                if (jobType == ballancerTask.MaxPriority)
//                {
//                    Thread.Sleep(1000);
//                }
            } while (run);
        }

        public Task StartAsync()
        {
            return Task.Factory.StartNew(Start);
        }

        public void Dispose()
        {
            run = false;
            var toDispose = remoteMachines as IDisposable;
            if (toDispose != null)
                toDispose.Dispose();
        }

		private TJobType CpuUsageToJobType(float cpuAvgUsage)
        {
			var workloads = ballancerTask.JobWorkloads;
			//TODO test
			return workloads.Where (x => 
				x.Value.Item1 <= cpuAvgUsage && x.Value.Item2 > cpuAvgUsage
			).Select (x=>x.Key).Single ();
        }

        private static bool IsValidForMachine(int taskId, int machinesCount, int currentMachineId,
            HashSet<int> unaviableHosts)
        {
            int count = machinesCount + 1;
            bool isThisMachine = taskId%count == currentMachineId%count;
            if (isThisMachine)
            {
                return true;
            }
            foreach (int unreachableHost in unaviableHosts)
            {
                int newHostId;
                int newtaskId = taskId;
                int i = 0;
                do
                {
                    int tmpId = newtaskId + unreachableHost + i++;
                    newHostId = GetMachineIdForTask(tmpId, machinesCount);
                    if (newHostId == currentMachineId)
                    {
                        return true;
                    }
                } while (unaviableHosts.Contains(newHostId) || newHostId == 0);
            }
            return false;
        }

        private static int GetMachineIdForTask(int taskId, int machinesCount)
        {
            int count = machinesCount + 1;
            return taskId%count;
        }
    }
}