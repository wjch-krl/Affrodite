using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Afrodite.Abstract;
using Afrodite.Concrete;
using Afrodite.Connection;

namespace Afrodite
{
    public static class Configurator<T>
    {
        private static LoadBallancer<T> _ballancer;



        private static LocalComponent<T> _localTaskRunner;


        private static SlaveRemoteEndpoiont<T> _slaveRemoteEndpoiont; 




        public static IConfig Config { get; private set; }
        
        public static void SetConfigPath(string path)
        {
            Config = new ConfigFileReader(path).ReadConfig();
            _localTaskRunner = new LocalComponent<T>(Config.MachineId);
        }

        public static void SetPriotitesRange(int maxPriority)
        {
            _ballancer = new LoadBallancer<T>(maxPriority);
        }

        public static void InitDbConn(string className, string connectionString)
        {
            IDbConnection conn = Deflector.Create<IDbConnection>(className);
            conn.ConnectionString = connectionString;
            _ballancer.DbConnection = conn;
        }

        public static void RegisterMasterAction(Func<int, T> action)
        {
            _ballancer.MasterAction = action;
        }

        public static void RegisterSlaveAction(Func<T, bool> action)
        {
            _ballancer.SlaveAction = action;
            _localTaskRunner.TaskFunc = action;
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
                return Task.Factory.StartNew(() =>
                {
                    _ballancer.Start();
                });
            }
        }
    }
}