using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Afrodite.Abstract;
using Afrodite.Concrete;
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
        private bool run;

        public DatabaseMachineStateMenager(IEnumerable<IHost> hosts, IHost localhost, IDbConnection connection,
            string statusTableName, string machinesTablename)
        {
            this.hosts = hosts.ToArray();
            this.localhost = localhost;
            this.connection = connection;
            this.statusTableName = statusTableName;
            this.machinesTablename = machinesTablename;
            run = true;
            Task.Factory.StartNew(() =>
            {
                do
                {
                    UpdateStatus();
                    Thread.Sleep(5000);
                } while (run);
            });
        }

        private int UpdateStatus()
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = string.Format("update {0} set lbm_lastseen = now() where lbm_machineid = {1};", machinesTablename, localhost.MachineNumber);
                return command.ExecuteNonQuery();
            }
        }

        public IList<IHost> AviableHosts()
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    string.Format("Select * from {0} where lbm_lastseen > now() - '1 minute'::interval; ",
                        machinesTablename);
                var reader = command.ExecuteReader();
                return GetHosts(reader);
            }
        }

        private IList<IHost> GetHosts(IDataReader reader)
        {
            var list = new List<IHost>();
            while (reader.Read())
            {
                list.Add(new Host
                {
                    IpOrHostname = reader.GetString(reader.GetOrdinal("lbm_iporhostname")),
                    MachineNumber = reader.GetInt32(reader.GetOrdinal("lbm_machineid"))
                });
            }
            return list;
        }

        public int Count
        {
            get { return hosts.Length + 1; }
        }

        public IEnumerable<int> UnaviableHosts()
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    string.Format("Select * from {0} where lbm_lastseen < now() - '1 minute'::interval; ",
                        machinesTablename);
                var reader = command.ExecuteReader();
                return GetHostsIds(reader);
            }
        }

        private IEnumerable<int> GetHostsIds(IDataReader reader)
        {
            var list = new List<int>();
            while (reader.Read())
            {
                list.Add(reader.GetInt32(reader.GetOrdinal("lbm_machineid")));
            }
            return list;
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