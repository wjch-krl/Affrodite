using System;
using System.Collections.Generic;

namespace Afrodite.Abstract
{
	public interface IComponentState<TJob>
	{
		/// <summary>
		/// Gets the machine identifier.
		/// </summary>
		/// <value>The machine identifier.</value>
		Guid MachineId {get;}
		/// <summary>
		/// Gets the aviable memory (In MiB).
		/// </summary>
		/// <value>The aviable memory value.</value>
		ulong AviableMemory {get;}
		/// <summary>
		/// Gets the cpu usage (Percentage per core).
		/// </summary>
		/// <value>The cpu usage.</value>
		Dictionary<string,float> CpuUsages {get;}
		/// <summary>
		/// Gets the active jobs.
		/// </summary>
		/// <value>The active jobs.</value>
		IEnumerable<IJob<TJob>> ActiveJobs{ get; }
		/// <summary>
		/// Gets the machine number.
		/// </summary>
		/// <value>The machine number.</value>
		int MachineNumber { get;}
		/// <summary>
		/// Gets or sets the free disk space.
		/// </summary>
		/// <value>The free disk space.</value>
		ulong FreeDiskSpace{ get; }
	}
}