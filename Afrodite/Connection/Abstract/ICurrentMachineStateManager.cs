using System;
using Afrodite.Abstract;
using Afrodite.Concrete;

namespace Afrodite.Connection.Abstract
{
	public interface ICurrentMachineStateManager<T>
	{
	    IComponentState<T> CurrentState();
	}

    class CurrentMachineStateManager<T> : ICurrentMachineStateManager<T>
    {
        private readonly IPerformanceManager performanceManager;
        private readonly IComponentProperties machineInfo;

        public CurrentMachineStateManager(IPerformanceManager performanceManager,IComponentProperties machineInfo)
        {
            this.performanceManager = performanceManager;
            this.machineInfo = machineInfo;
        }

        public IComponentState<T> CurrentState()
        {
            throw new NotImplementedException();
          //  return new ComponentState<T>(performanceManager.GetCpusUsage(),machineInfo.MachineId,performanceManager.GetTotalMemory()-performanceManager.GetAviableMemory(),);
        }
    }
}

