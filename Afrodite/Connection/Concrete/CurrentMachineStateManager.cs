using Afrodite.Abstract;
using Afrodite.Concrete;
using Afrodite.Connection.Abstract;

namespace Afrodite.Connection.Concrete
{
    class CurrentMachineStateManager<TJob> : ICurrentMachineStateManager<TJob>
    {
        private readonly IPerformanceManager performanceManager;
        private readonly IComponentProperties machineInfo;
        private ITaskRunner<TJob> localBallancer;

        public CurrentMachineStateManager(IPerformanceManager performanceManager,IComponentProperties machineInfo,
            ITaskRunner<TJob> localBallancer)
        {
            this.performanceManager = performanceManager;
            this.machineInfo = machineInfo;
            this.localBallancer = localBallancer;
        }

        public IComponentState<TJob> CurrentState()
        {
            return new ComponentState<TJob>(performanceManager.GetCpusUsage(),machineInfo.MachineId,
                performanceManager.GetTotalMemory()-performanceManager.GetAviableMemory(),
                localBallancer.GetActiveJobs());
        }
    }
}