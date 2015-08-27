using System;
using System.Collections.Generic;
using Afrodite.Abstract;

namespace Afrodite.Concrete
{
    public class ComponentState<TJob> : IComponentState<TJob>
    {
        public ComponentState(Dictionary<string, float> cpuUsages, Guid machineId, ulong usedMemory, 
            IEnumerable<IJob<TJob>> activeJobs)
        {
            CpuUsages = cpuUsages;
            MachineId = machineId;
            UsedMemory = usedMemory;
            ActiveJobs = activeJobs;
        }

        public Guid MachineId { get; private set; }
        public ulong UsedMemory { get; private set; }
        public Dictionary<string, float> CpuUsages { get; private set; }
        public IEnumerable<IJob<TJob>> ActiveJobs { get; private set; }
    }
}