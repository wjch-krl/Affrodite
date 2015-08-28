using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Afrodite;

namespace AffroditeP2P
{
    internal class RemoteMachinesManager : IDisposable
    {
        private readonly IList<Tuple<RemoteHost, IPEndPoint>> hosts;
        private Pinger pinger;
        private RemoteHost[] activeHosts;
        private bool run;

        public RemoteHost[] AviableHosts
        {
            get
            {
                lock (activeHosts)
                    return activeHosts.ToArray();
            }
        }

        public RemoteMachinesManager(int port, int timeout, IEnumerable<RemoteHost> hosts)
        {
            this.activeHosts = hosts.ToArray();
            this.hosts = activeHosts.Select(x => new Tuple<RemoteHost, IPEndPoint>(x, new IPEndPoint(IPAddress.Parse(x.MachineIp), x.PingerPort))).ToArray();
            this.pinger = new Pinger(port, timeout);
            Task.Factory.StartNew(PingAllhHosts);
        }

        private void PingAllhHosts()
        {
            do
            {
                var tmp = (hosts.Where(remoteHost => pinger.Ping(remoteHost.Item2))
                        .Select(remoteHost => remoteHost.Item1)).ToArray();
                lock (activeHosts)
                {
                    activeHosts = tmp;
                }
            } while (run);
        }


        public void Dispose()
        {
            run = false;
            pinger.Dispose();
        }
    }
}