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
				var records = csv.GetRecords<ComponentState<object>> ();
				return records;
			}
		}
	}

}