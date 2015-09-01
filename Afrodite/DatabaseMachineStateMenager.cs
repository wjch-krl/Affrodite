using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Afrodite.Abstract;
using Afrodite.Connection;

namespace Afrodite
{
    internal class DatabaseMachineStateMenager : IRemoteMachinesManager, IDisposable
    {
        private readonly IHost[] hosts;
        private readonly IHost localhost;
        private readonly IDbConnection connection;
        private readonly string statusTableName;
        private readonly string machinesTablename;

        public DatabaseMachineStateMenager(IEnumerable<IHost> hosts, IHost localhost, IDbConnection connection,
            string statusTableName, string machinesTablename)
        {
            this.hosts = hosts.ToArray();
            this.localhost = localhost;
            this.connection = connection;
            this.statusTableName = statusTableName;
            this.machinesTablename = machinesTablename;
        }


        public IList<IHost> AviableHosts()
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = string.Format("Select * from {0} where lbm_lastseen > now() - '1 minute'::interval; ", 
                                                    machinesTablename);
                var reader = command.ExecuteReader();
                return GetHosts(reader);
            }
        }

        private IList<IHost> GetHosts(IDataReader reader)
        {
            throw new NotImplementedException();

        }

        public int Count
        {
            get { return hosts.Length + 1; }
        }

        public IEnumerable<int> UnaviableHosts()
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = string.Format("Select * from {0} where lbm_lastseen < now() - '1 minute'::interval; ",
                                                    machinesTablename);
                var reader = command.ExecuteReader();
                return GetHostsIds(reader);
            }
        }

        private IEnumerable<int> GetHostsIds(IDataReader reader)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (connection != null)
            {
                connection.Dispose();
            }
        }
    }
}