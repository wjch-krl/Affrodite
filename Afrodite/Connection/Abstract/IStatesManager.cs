using System;

namespace Afrodite.Connection.Abstract
{
	public interface IStatesManager<T>
	{
		IMachineStateCollection<T> this [Guid key]
		{
			get;
			set;
		}
	}

}

