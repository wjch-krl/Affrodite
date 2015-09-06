using System;
using System.Collections.Generic;
using Afrodite.Abstract;

namespace Afrodite.Concrete
{
	public class ComponentState<TJob> : IComponentState<TJob>
    {
        public ComponentState(Dictionary<string, float> cpuUsages, Guid machineId, ulong usedMemory, 
			IEnumerable<IJob<TJob>> activeJobs,ulong freeDiskSpace,int machineNumber)
        {
            CpuUsages = cpuUsages;
            MachineId = machineId;
            AviableMemory = usedMemory;
            ActiveJobs = activeJobs;
			FreeDiskSpace = freeDiskSpace;
			MachineNumber = machineNumber;
        }

        public Guid MachineId { get; private set; }
        public ulong AviableMemory { get; private set; }
        public Dictionary<string, float> CpuUsages { get; private set; }
        public IEnumerable<IJob<TJob>> ActiveJobs { get; private set; }

		public int MachineNumber{ get; private set; }

		public ulong FreeDiskSpace{ get; private set; }
    }
}