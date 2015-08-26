using System.Collections.Generic;
using Afrodite.Abstract;

namespace Afrodite.Concrete
{
    public class ComponentState : IComponentState
    {
        public ComponentState(float[] cpuUsages, int machineId, int usedMemory, 
            int numberOfRunningJobs, IEnumerable<IJob> activeJobs)
        {
            CpuUsages = cpuUsages;
            MachineId = machineId;
            UsedMemory = usedMemory;
            NumberOfRunningJobs = numberOfRunningJobs;
            ActiveJobs = activeJobs;
        }

        public int MachineId { get; private set; }
        public int UsedMemory { get; private set; }
        public float[] CpuUsages { get; private set; }
        public int NumberOfRunningJobs { get; private set; }
        public IEnumerable<IJob> ActiveJobs { get; private set; }
    }
}