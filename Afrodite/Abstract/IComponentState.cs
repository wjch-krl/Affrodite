using System.Collections.Generic;

namespace Afrodite.Abstract
{
	public interface IComponentState
	{
		/// <summary>
		/// Gets the machine identifier.
		/// </summary>
		/// <value>The machine identifier.</value>
		int MachineId {get;}
		/// <summary>
		/// Gets the used memory (In MiB).
		/// </summary>
		/// <value>The used memory.</value>
		int UsedMemory{get;}
		/// <summary>
		/// Gets the cpu usage (Percentage per core).
		/// </summary>
		/// <value>The cpu usage.</value>
		float[] CpuUsages {get;}
		/// <summary>
		/// Gets the number of running jobs.
		/// </summary>
		/// <value>The number of running jobs.</value>
		int NumberOfRunningJobs {get;}
		/// <summary>
		/// Gets the active jobs.
		/// </summary>
		/// <value>The active jobs.</value>
		IEnumerable<IJob> ActiveJobs{ get; }
	}
}