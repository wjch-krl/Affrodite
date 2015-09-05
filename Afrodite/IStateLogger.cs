using System;
using System.Collections.Generic;
using System.Data;
using Afrodite.Abstract;
using Afrodite.Common;
using System.Linq;
using CsvHelper;
using System.IO;

namespace Afrodite
{
	public interface IStateLogger
	{
		void SaveState<T> (IComponentState<T> state);

		IEnumerable<IComponentState<object>> GetAllStates ();
	}

	public class CsvFileStateLogger : IStateLogger
	{
		string path;

		public CsvFileStateLogger (string path)
		{
			if (path == null)
				throw new ArgumentNullException ("path");
			this.path = path;
		}

		public void SaveState<T> (IComponentState<T> state)
		{
			using (var textWriter = new StreamWriter (path, true))
			{
				var csv = new CsvWriter (textWriter);
				csv.WriteRecord (state);
			}
		}

		public IEnumerable<IComponentState<object>> GetAllStates ()
		{
			using (var textReader = new StreamReader (path))
			{
				var csv = new CsvReader (textReader);
				var records = csv.GetRecords<IComponentState<object>> ();
				return records;
			}
		}
	}


	internal class DatabaseStateLogger : IStateLogger, IDisposable
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
			cmd.Parameters.Add ();

			param = cmd.CreateParameter ();
			param.DbType = DbType.Double;
			param.ParameterName = "3";
			param.Value = state.CpuUsages.Average (x => (double)x.Value);
			cmd.Parameters.Add (param);

			param = cmd.CreateParameter ();
			param.DbType = DbType.UInt64;
			param.ParameterName = "4";
			param.Value = state.UsedMemory;
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