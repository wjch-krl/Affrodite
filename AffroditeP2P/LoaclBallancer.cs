using System;
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
                foreach (var job in toDos)
                {
                    var aviableCount = aviableMachines.Count;
                    var tmp = aviableMachines.OrderBy(x=>x.MachineNumber).Select((x, i) => new Tuple<IHost, int>(x, i));
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
    }
}
