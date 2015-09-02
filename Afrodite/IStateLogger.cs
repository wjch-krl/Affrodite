using System;
using System.Collections.Generic;
using System.Data;
using Afrodite.Abstract;
using Afrodite.Common;

namespace Afrodite
{
    public interface IStateLogger
    {
        void SaveState<T>(IComponentState<T> state);
        IEnumerable<IComponentState<object>> GetAllStates();
    }

    internal class FileStateLogger : IStateLogger
    {
        public void SaveState<T>(IComponentState<T> state)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IComponentState<object>> GetAllStates()
        {
            throw new NotImplementedException();
        }
    }


    internal class DatabaseStateLogger : IStateLogger, IDisposable
    {
        private readonly IDbConnection connection;

        public DatabaseStateLogger(IDbConnection connection, string stateTableName)
        {
            this.connection = connection;
            if (!connection.TableExists(stateTableName))
            {
                throw new InvalidOperationException(string.Format("{0} doesn't exists.", stateTableName));
            }
        }

        public void SaveState<T>(IComponentState<T> state)
        {
            connection.ExecuteNonQuery(
                String.Format(
                    "insert into {0} (lbs_statusdate,loadballancer_machines_lbm_id,lbs_cpuussage,lbs_freememory,lbs_freediskspace) values (CURRENT_TIMESTAMP,"));
        }

        public IEnumerable<IComponentState<object>> GetAllStates()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}