using System;
using System.Collections.Generic;
using System.Data;
using Afrodite.Abstract;

namespace Afrodite.Concrete
{
    class LoadBallancer<T> : IBallancer<T>
    {
        private readonly List<IComponent> componets;

        public LoadBallancer()
        {
            this.componets = new List<IComponent>();
        }

        public IRegistrationStatus RegisterComponent(IComponent component)
        {
            this.componets.Add(component);
            return new RegistrationStatus();
        }

        public IEnumerable<IComponent> GetComponents()
        {
            return componets;
        }

        public void Start()
        {
            DistributeWorkload();
        }

        private void DistributeWorkload()
        {
            
        }

        public IDbConnection DbConnection { get; set; }
        public Func<int, T> MasterAction { get; set; }
        public Func<T, bool> SlaveAction { get; set; }
        public Action MasterFailureAction { get; set; }
    }
}
