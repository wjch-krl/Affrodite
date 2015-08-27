using Afrodite.Abstract;
using Afrodite.Concrete;
using Afrodite.Connection.Abstract;

namespace Afrodite.Connection.Concrete
{
    class CurrentMachineStateManager<T> : ICurrentMachineStateManager<T>
    {
        private readonly IPerformanceManager performanceManager;
        private readonly IComponentProperties machineInfo;
        private ITaskRunner<T> localBallancer;

        public CurrentMachineStateManager(IPerformanceManager performanceManager,IComponentProperties machineInfo,
            ITaskRunner<T> localBallancer)
        {
            this.performanceManager = performanceManager;
            this.machineInfo = machineInfo;
            this.localBallancer = localBallancer;
        }

        public IComponentState<T> CurrentState()
        {
            return new ComponentState<T>(performanceManager.GetCpusUsage(),machineInfo.MachineId,
                performanceManager.GetTotalMemory()-performanceManager.GetAviableMemory(),
                localBallancer.GetActiveJobs());
        }
    }
}