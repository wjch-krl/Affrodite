using System;
using System.Collections.Generic;

namespace Afrodite
{
	public interface IComponentState
	{
		int UsedMemory{get;}
		float CpuUsage {get;}
		int NumberOfRunningJobs {get;}
		IEnumerable<IJob> ActiveJobs{ get; }
	}
}