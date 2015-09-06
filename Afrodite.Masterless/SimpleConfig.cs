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

        /// <summary>
        /// Starts load balancer 
        /// </summary>
        /// <typeparam name="T">Task type</typeparam>
        /// <param name="getTasksFunc">Func that is used to get aviable jobs with given type </param>
        /// <param name="startTaskFunc">Func that is used to start new job</param>
        /// <param name="maxPrior"></param>
        /// <returns>System.Threading.Task that runs the load ballancer</returns>
		public Task StrartLoadBallancer<TJob,TJobType>(Func<TJobType, IEnumerable<TJob>> getTasksFunc, Func<TJob, bool> startTaskFunc,
			IEnumerable<Tuple<TJobType,Tuple<int,int>>> jobTypesMapping)
        {
            var config = reader.ReadConfig();
			var ballancerTask = new BallancerDelagateTask<TJob,TJobType>(getTasksFunc, startTaskFunc, jobTypesMapping);
//            var machineM = new DatabaseMachineStateMenager(config.Hosts, config,
//                new SqlConnection("Server=PC-WKROL;Database=test;Integrated Security=True;"),
//                "loadballancer_machines");
            var machineM = new RemoteMachinesManager(config.PingerPort, config.Timeout, config.Hosts, config);

			var ballancer = new LocalBallancer<TJob,TJobType>(ballancerTask, new PerformanceManager(), machineM
                ,config.MachineNumber);
            return ballancer.StartAsync();
        }

        /// <summary>
        /// Starts load balancer 
        /// </summary>
        /// <typeparam name="T">Task type</typeparam>
        /// <param name="ballancerTask">Ballancer task to run</param>
        /// <returns>System.Threading.Task that runs the load ballancer</returns>
		public Task StrartLoadBallancer<TJob,TJobType>(IBallancerTask<TJob,TJobType> ballancerTask)
        {
            var config = reader.ReadConfig();
            var machineM = new RemoteMachinesManager(config.PingerPort, config.Timeout, config.Hosts, config);
			var ballancer = new LocalBallancer<TJob,TJobType>(ballancerTask, new PerformanceManager(),
                machineM, config.MachineNumber);
            return ballancer.StartAsync();
        }
    }
}