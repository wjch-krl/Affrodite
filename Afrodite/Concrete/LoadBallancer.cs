using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Afrodite.Abstract;
using Afrodite.Connection;

namespace Afrodite.Concrete
{
    public class LoadBallancer<TJob> : IBallancer<TJob>
    {
        private readonly int maxPriority;
        private readonly List<IComponent<TJob>> componets;
        private readonly MasterRemoteEndpoint<TJob> serverConnection;

        public LoadBallancer(int maxPriority)
        {
            this.maxPriority = maxPriority;
            this.componets = new List<IComponent<TJob>>();
        }

        public IRegistrationStatus RegisterComponent(IComponent<TJob> component)
        {
            this.componets.Add(component);
            return new RegistrationStatus();
        }

        public IEnumerable<IComponent<TJob>> GetComponents()
        {
            return componets;
        }

        public void Start()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    DistributeWorkload();
                    Thread.Sleep(1000);
                }
            });
        }

        private void DistributeWorkload()
        {
            var aviableComponets = ProcessComponents(componets);
            foreach (var compGroups in aviableComponets)
            {
                foreach (var component in compGroups)
                {
                    var newJob = MasterAction(compGroups.Key);
                    var job = component.StartJob(newJob);
                }
            }
        }

        private ILookup<int, IComponent<TJob>> ProcessComponents(List<IComponent<TJob>> components)
        {
            return components.ToLookup(x => CpuUsageToPriority(x.State.CpuUsages.Values.Average()), x => x);
        }

        private int CpuUsageToPriority(float cpuAvgUsage)
        {
            double granulation = 100.0/maxPriority;
            double interval = cpuAvgUsage/granulation;
            return Convert.ToInt32(interval);
        }

        public IDbConnection DbConnection { get; set; }
        public Func<int, TJob> MasterAction { get; set; }
        public Func<TJob, bool> StartTaskAction { get; set; }
        public Func<TJob, bool> StopTaskAction { get; set; }
        public Func<TJob, bool> PauseTaskAction { get; set; }
        public Func<TJob, bool> ResumeTaskAction { get; set; }

        public Action MasterFailureAction { get; set; }
    }
}