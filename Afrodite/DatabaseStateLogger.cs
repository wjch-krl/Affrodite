using System;
using System.Collections.Generic;
using System.Data;
using Afrodite.Abstract;
using Afrodite.Common;
using System.Linq;
using CsvHelper;
using System.IO;
using Afrodite.Concrete;

namespace Afrodite
{

	public class DatabaseStateLogger : IStateLogger, IDisposable
	{
		private readonly IDbConnection connection;

		public DatabaseStateLogger (IDbConnection connection, string stateTableName)
		{
			this.connection = connection;
			if (!connection.TableExists (stateTableName))
			{
				throw new InvalidOperationException (string.Format ("{0} doesn't exists.", stateTableName));
			}
		}

		public void SaveState<T> (IComponentState<T> state)
		{
			var cmd = connection.CreateCommand ();
			cmd.CommandText = 
				"insert into {0} (lbs_statusdate,loadballancer_machines_lbm_id," +
			"lbs_cpuussage,lbs_freememory,lbs_freediskspace) " +
			"values ($1,$2,$3,$4,$5)";
			IDbDataParameter param = cmd.CreateParameter ();
			param.DbType = DbType.DateTime;
			param.ParameterName = "1";
			param.Value = DateTime.UtcNow;
			cmd.Parameters.Add (param);

			param = cmd.CreateParameter ();
			param.DbType = DbType.UInt64;
			param.ParameterName = "2";
			param.Value = state.MachineNumber;
			cmd.Parameters.Add (param);

			param = cmd.CreateParameter ();
			param.DbType = DbType.Double;
			param.ParameterName = "3";
			param.Value = state.CpuUsages.Average (x => (double)x.Value);
			cmd.Parameters.Add (param);

			param = cmd.CreateParameter ();
			param.DbType = DbType.UInt64;
			param.ParameterName = "4";
			param.Value = state.AviableMemory;
			cmd.Parameters.Add (param);

			param = cmd.CreateParameter ();
			param.DbType = DbType.UInt64;
			param.ParameterName = "5";
			param.Value = state.FreeDiskSpace;
			cmd.Parameters.Add (param);

			cmd.ExecuteNonQuery ();
		}

		public IEnumerable<IComponentState<object>> GetAllStates ()
		{
			throw new NotImplementedException ();
		}

		public void Dispose ()
		{
			connection.Dispose ();
		}
	}
}