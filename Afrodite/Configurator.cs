using System;
using System.Data;
using Afrodite.Concrete;

namespace Afrodite
{
    public static class Configurator<T>
    {
        private static LoadBallancer<T> _ballancer;

        static Configurator()
        {
            _ballancer = new LoadBallancer<T>();
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
        }

        public static void RegisterMasterFailureAction(Action action)
        {
            _ballancer.MasterFailureAction = action;
        }
    }
}