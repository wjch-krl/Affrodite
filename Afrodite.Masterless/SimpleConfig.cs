using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Afrodite.Concrete;
using Afrodite.Connection;

namespace AffroditeP2P
{
    public class SimpleConfig
    {
        private ConfigFileReader reader;

        public SimpleConfig(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            reader = new ConfigFileReader(path);
        }

        public Task StrartLoadBallancer<T>(Func<int, IEnumerable<T>> getTasksFunc, Func<T, bool> startTaskFunc,
            int maxPrior)
        {
            var config = reader.ReadConfig();
            var ballancerTask = new BallancerDelagateTask<T>(getTasksFunc, startTaskFunc, maxPrior);
            var ballancer = new LocalBallancer<T>(ballancerTask, new PerformanceManager(),
                new RemoteMachinesManager(config.PingerPort, config.Timeout, config.Hosts, config),config.MachineNumber);
            return ballancer.StartAsync();
        }

        public Task StrartLoadBallancer<T>(IBallancerTask<T> ballancerTask)
        {
            var config = reader.ReadConfig();
            var ballancer = new LocalBallancer<T>(ballancerTask, new PerformanceManager(),
                new RemoteMachinesManager(config.PingerPort, config.Timeout, config.Hosts, config), config.MachineNumber);
            return ballancer.StartAsync();
        }
    }
}