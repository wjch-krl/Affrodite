using System;
using System.Collections.Generic;
using Afrodite.Abstract;

namespace Afrodite.Concrete
{
    public class ComponentState<T> : IComponentState<T>
    {
        public ComponentState(Dictionary<string, float> cpuUsages, Guid machineId, ulong usedMemory, 
            int numberOfRunningJobs, IEnumerable<IJob<T>> activeJobs)
        {
            CpuUsages = cpuUsages;
            MachineId = machineId;
            UsedMemory = usedMemory;
            NumberOfRunningJobs = numberOfRunningJobs;
            ActiveJobs = activeJobs;
        }

        public Guid MachineId { get; private set; }
        public ulong UsedMemory { get; private set; }
        public Dictionary<string, float> CpuUsages { get; private set; }
        public int NumberOfRunningJobs { get; private set; }
        public IEnumerable<IJob<T>> ActiveJobs { get; private set; }
    }
}