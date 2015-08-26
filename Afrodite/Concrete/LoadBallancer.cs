using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Afrodite.Abstract;

namespace Afrodite.Concrete
{
    internal class LoadBallancer<T> : IBallancer<T>
    {
        private readonly List<IComponent<T>> componets;

        public LoadBallancer()
        {
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
            DistributeWorkload();
        }

        private void DistributeWorkload()
        {
            var newJob = MasterAction(1);
            var job = componets.First().StartJob(newJob);

        }

        public IDbConnection DbConnection { get; set; }
        public Func<int, T> MasterAction { get; set; }
        public Func<T, bool> SlaveAction { get; set; }
        public Action MasterFailureAction { get; set; }
    }
}