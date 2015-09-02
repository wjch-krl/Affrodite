using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Afrodite.Abstract;
using Afrodite.Common;
using Afrodite.Concrete;
using Afrodite.Connection;

namespace Afrodite
{
    public class DatabaseMachineStateMenager : IRemoteMachinesManager, IDisposable
    {
        private readonly IHost[] hosts;
        private readonly IHost localhost;
        private readonly IDbConnection connection;
        private readonly string machinesTablename;
        private bool run;

        public DatabaseMachineStateMenager(IEnumerable<IHost> hosts, IHost localhost, IDbConnection connection,
            string machinesTablename)
        {
            this.hosts = hosts.ToArray();
            this.localhost = localhost;
            this.connection = connection;
            this.machinesTablename = machinesTablename;
            run = true;
            CancellationToken token = new CancellationToken();
            CreateDbEntry();
            Task.Factory.StartNew(() =>
            {
                token.ThrowIfCancellationRequested();
                do
                {
                    CheckIn();
                    Thread.Sleep(5000);
                } while (run);
            },token);
        }

        private void CreateDbEntry()
        {
            try
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        string.Format("insert into {0} (lbm_machineid) values ({1})",
                            machinesTablename, localhost.MachineNumber);
                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                connection.Close();
            }
        }

        private int CheckIn()
        {
            try
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        string.Format("update {0} set lbm_lastseen = SYSDATETIME() where lbm_machineid = {1};",
                            machinesTablename, localhost.MachineNumber);
                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Logger.LoggError(ex,Logger.GetCurrentMethod());
                return -1;
            }
            finally
            {
                connection.Close();
            }
        }

        public IList<IHost> AviableHosts()
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    string.Format("Select * from {0} where lbm_lastseen > SYSDATETIME() - DATEADD(minute, -1, SYSDATETIME());",
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
            try
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        string.Format("Select * from {0} where lbm_lastseen < DATEADD(minute, -1, SYSDATETIME()); ",
                            machinesTablename);
                    var reader = command.ExecuteReader();
                    return GetHostsIds(reader);
                }
            }
            finally
            {
                connection.Close();
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