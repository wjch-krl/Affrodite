using System;

namespace Afrodite.Connection
{
	public interface IMachineJob<T>
	{
		Guid Guid { get; set; }
		DateTime StartTime { get; set; }
		T JobDetails {get;set;}
	}

}

