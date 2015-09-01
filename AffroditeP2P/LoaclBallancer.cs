using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Afrodite;
using Afrodite.Abstract;
using Afrodite.Connection;

namespace AffroditeP2P
{
    public class LoaclBallancer<TJob> : IDisposable
    {
        private readonly IBallancerTask<TJob> ballancerTask;
        private IRemoteMachinesManager remoteMachines;
        private IPerformanceManager performanceManager;
        private bool run;

        public LoaclBallancer(IBallancerTask<TJob> ballancerTask, IPerformanceManager performanceManager, IRemoteMachinesManager remoteMachines)
        {
            this.ballancerTask = ballancerTask;
            this.performanceManager = performanceManager;
            this.remoteMachines = remoteMachines;
        }

        public void Start()
        {
            run = true;
            do
            {
                var cpuUsage = performanceManager.GetAvgCpusUsage();
                var priority = CpuUsageToPriority(cpuUsage);
                var toDos = ballancerTask.GetJobs(priority);
                var aviableMachines = remoteMachines.AviableHosts;
                var aviableCount = aviableMachines.Count;
                var tmp = aviableMachines.OrderBy(x => x.MachineNumber).Select((x, i) => new Tuple<IHost, int>(x, i));
                var current = aviableMachines.First(x => x.MachineNumber == 1);
                foreach (var job in toDos)
                {
                    
                    if(true)
                        ballancerTask.StartJob(job);

                    //TODO Critial - select job
                }
            } while (run);
        }

        public Task StartAsync()
        {
            return Task.Factory.StartNew(Start);
        }

        public void Dispose()
        {
            run = false;
            remoteMachines.Dispose();;
        }

        private int CpuUsageToPriority(float cpuAvgUsage)
        {
            double granulation = 100.0 / ballancerTask.MaxPriority;
            double interval = cpuAvgUsage / granulation;
            return Convert.ToInt32(interval);
        }

        public static bool IsValidForMachine(int taskId,int machinesCount, int currentMachineId)
		{
			int count = machinesCount + 1;
			return taskId % count == currentMachineId % count;
		}

		public static bool IsValidForMachine(int taskId,int machinesCount, int currentMachineId, HashSet<int> unaviableHosts)
		{
			int count = machinesCount + 1;
			bool isThisMachine = taskId % count == currentMachineId % count;
			if (isThisMachine)
			{
				return true;
			}
			foreach (int unreachableHost in unaviableHosts)
			{
				int newId = taskId;
				do
				{
					int tmpId = newId + unreachableHost + 1;
					newId = GetMachineIdForTask (tmpId, machinesCount);
					if (newId == currentMachineId)
					{
						return true;
					} 
				} while (unaviableHosts.Contains (newId));//TODO check if could become endless loop
			}
			return false;
		}

		public static int GetMachineIdForTask (int taskId, int machinesCount)
		{
			int count = machinesCount + 1;
			return taskId % count;
		}
	}
}
