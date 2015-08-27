using System;
using System.Collections.Generic;

namespace Afrodite.Abstract
{
	public interface IComponentState<T>
	{
		/// <summary>
		/// Gets the machine identifier.
		/// </summary>
		/// <value>The machine identifier.</value>
		Guid MachineId {get;}
		/// <summary>
		/// Gets the used memory (In MiB).
		/// </summary>
		/// <value>The used memory.</value>
		ulong UsedMemory{get;}
		/// <summary>
		/// Gets the cpu usage (Percentage per core).
		/// </summary>
		/// <value>The cpu usage.</value>
		Dictionary<string,float> CpuUsages {get;}
		/// <summary>
		/// Gets the active jobs.
		/// </summary>
		/// <value>The active jobs.</value>
		IEnumerable<IJob<T>> ActiveJobs{ get; }
	}
}