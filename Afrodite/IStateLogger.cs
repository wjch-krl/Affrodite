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
	public interface IStateLogger
	{
		void SaveState<T> (IComponentState<T> state);

		IEnumerable<IComponentState<object>> GetAllStates ();
	}



}