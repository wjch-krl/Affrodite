using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Afrodite;
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
            var machineM = new DatabaseMachineStateMenager(config.Hosts, config, 
                new SqlConnection("Server=PC-WKROL;Database=test;"),
                "loadballancer_machines", "loadballancer_status");

            var ballancer = new LocalBallancer<T>(ballancerTask, new PerformanceManager(), machineM
                ,config.MachineNumber);
            return ballancer.StartAsync();
        }

        public Task StrartLoadBallancer<T>(IBallancerTask<T> ballancerTask)
        {
            var config = reader.ReadConfig();
            var machineM = new RemoteMachinesManager(config.PingerPort, config.Timeout, config.Hosts, config);
            var ballancer = new LocalBallancer<T>(ballancerTask, new PerformanceManager(),
                machineM, config.MachineNumber);
            return ballancer.StartAsync();
        }
    }
}