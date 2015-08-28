using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AffroditeP2P
{
    public class LoaclBallancer<TJob> : IDisposable
    {
        private readonly IBallancerTask<TJob> ballancerTask;
        private RemoteMachinesManager remoteMachines;
        private bool run;
        public LoaclBallancer(IBallancerTask<TJob> ballancerTask)
        {
            this.ballancerTask = ballancerTask;
        }

        public void Start()
        {
            run = true;
            do
            {
                var aviableMachines = remoteMachines.AviableHosts;
//                var toDos = ballancerTask.GetJobs();
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
    }
}
