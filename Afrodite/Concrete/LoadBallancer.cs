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
    public class LoadBallancer<T> : IBallancer<T>
    {
        private readonly int maxPriority;
        private readonly List<IComponent<T>> componets;
        private readonly MasterRemoteEndpoint<T> serverConnection; 

        public LoadBallancer(int maxPriority)
        {
            this.maxPriority = maxPriority;
            this.componets = new List<IComponent<T>>();
        }

        public IRegistrationStatus RegisterComponent(IComponent<T> component)
        {
            this.componets.Add(component);
            return new RegistrationStatus();
        }

        public IEnumerable<IComponent<T>> GetComponents()
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

        private ILookup<int,IComponent<T>> ProcessComponents(List<IComponent<T>> components)
        {
            return components.ToLookup(x => CpuUsageToPriority(x.State.CpuUsages.Values.Average()) , x => x);
        }

        private int CpuUsageToPriority(float cpuAvgUsage)
        {
            double granulation = 100.0/maxPriority;
            double interval = cpuAvgUsage/granulation;
            return Convert.ToInt32(interval);
        }

        public IDbConnection DbConnection { get; set; }
        public Func<int, T> MasterAction { get; set; }
        public Func<T, bool> SlaveAction { get; set; }
        public Action MasterFailureAction { get; set; }
    }
}