using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Afrodite.Abstract;

namespace Afrodite.Connection
{
    public class RemoteMachinesManager : IDisposable, IRemoteMachinesManager
    {
        private readonly IHost localhost;
        private readonly IList<Tuple<IHost, IPEndPoint>> hosts;
        private Pinger pinger;
        private IList<IHost> inactiveHosts;
        private bool run;

        public IList<IHost> AviableHosts()
        {
            lock (inactiveHosts)
                return hosts.Where(x => !inactiveHosts.Contains(x.Item1)).Select(x => x.Item1).ToArray();
        }

        public int Count { get { return hosts.Count + 1; } }

        public IEnumerable<int> UnaviableHosts()
        {
            lock (inactiveHosts)
            {
                return inactiveHosts.Select(x => x.MachineNumber);
            }
        }

        public RemoteMachinesManager(int port, int timeout, IEnumerable<IHost> hosts,IHost localhost)
        {
            this.localhost = localhost;
            this.inactiveHosts = new List<IHost>();
            this.hosts = hosts.Select(x => new Tuple<IHost, IPEndPoint>(x, new IPEndPoint(IPAddress.Parse(x.IpOrHostname), x.PingerPort))).ToArray();
            this.pinger = new Pinger(port, timeout);
            Task.Factory.StartNew(PingAllHosts);
        }

        private void PingAllHosts()
        {
            do
            {
                var tmp = (hosts.Where(remoteHost => !pinger.Ping(remoteHost.Item2))
                        .Select(remoteHost => remoteHost.Item1)).ToList();
                lock (inactiveHosts)
                {
                    inactiveHosts = tmp;
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