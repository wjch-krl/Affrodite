using System;

namespace Afrodite.Connection.Abstract
{
	public interface IStatesManager<TJob>
	{
		IMachineStateCollection<TJob> this [Guid key]
		{
			get;
			set;
		}
	}

}

