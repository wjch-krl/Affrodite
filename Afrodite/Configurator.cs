using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Afrodite.Common;
using Afrodite.Concrete;
using Afrodite.Connection;

namespace Afrodite
{
    public static class Configurator<TJob>
    {
        private static LoadBallancer<TJob> _ballancer;

        private static LocalComponent<TJob> _localTaskRunner;

        private static SlaveRemoteEndpoiont<TJob> _slaveRemoteEndpoiont;

        public static LocalHost LocalHost { get; private set; }

        public static void SetConfigPath(string path)
        {
            LocalHost = new ConfigFileReader(path).ReadConfig();
			_localTaskRunner = new LocalComponent<TJob>(LocalHost.MachineId,LocalHost.MachineNumber);
        }

        public static void SetMaxPriotity(int maxPriority)
        {
            _ballancer = new LoadBallancer<TJob>(maxPriority);
        }

        public static void InitDbConn(string className, string connectionString)
        {
            IDbConnection conn = Deflector.Create<IDbConnection>(className);
            conn.ConnectionString = connectionString;
            _ballancer.DbConnection = conn;
        }

        public static void RegisterMasterAction(Func<int, TJob> action)
        {
            _ballancer.MasterAction = action;
        }

        public static void RegisterSlaveAction(Func<TJob, bool> startAction, Func<TJob, bool> pauseAction = null,
            Func<TJob, bool> stopAction = null, Func<TJob, bool> resumeAction = null)
        {
            _ballancer.StartTaskAction = startAction;
            _localTaskRunner.StartTaskAction = startAction;
            if (pauseAction == null)
            {
                pauseAction = arg => false;
            }
            _ballancer.PauseTaskAction = pauseAction;
            _localTaskRunner.PauseTaskAction = pauseAction;
            if (resumeAction == null)
            {
                resumeAction = arg => false;
            }
            _ballancer.ResumeTaskAction = resumeAction;
            _localTaskRunner.ResumeTaskAction = resumeAction;
            if (stopAction == null)
            {
                stopAction = arg => false;
            }
            _ballancer.StopTaskAction = stopAction;
            _localTaskRunner.StopTaskAction = stopAction;
        }

        public static void RegisterMasterFailureAction(Action action)
        {
            _ballancer.MasterFailureAction = action;
        }

        public static Task Start()
        {
            bool isMaster = true;
            if (isMaster)
            {
                _ballancer.RegisterComponent(_localTaskRunner);
                return Task.Factory.StartNew(() => { _ballancer.Start(); });
            }
            return Task.Factory.StartNew(() => { });
        }


        public static void RegisterMasterAction1(Func<int, IEnumerable<TJob>> action)
        {
            
        }   
    }
}