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
        private IList<IHost> activeHosts;
        private bool run;

        public IList<IHost> AviableHosts
        {
            get
            {
                lock (activeHosts)
                    return activeHosts.ToArray();
            }
        }

        public RemoteMachinesManager(int port, int timeout, IEnumerable<IHost> hosts,IHost localhost)
        {
            this.localhost = localhost;
            var enumerable = hosts as IList<IHost> ?? hosts.ToList();
            this.activeHosts = new List<IHost>(enumerable){localhost};
            this.hosts = enumerable.Select(x => new Tuple<IHost, IPEndPoint>(x, new IPEndPoint(IPAddress.Parse(x.IpOrHostname), x.PingerPort))).ToArray();
            this.pinger = new Pinger(port, timeout);
            Task.Factory.StartNew(PingAllHosts);
        }

        private void PingAllHosts()
        {
            do
            {
                var tmp = (hosts.Where(remoteHost => pinger.Ping(remoteHost.Item2))
                        .Select(remoteHost => remoteHost.Item1)).ToList();
                tmp.Add(localhost);
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