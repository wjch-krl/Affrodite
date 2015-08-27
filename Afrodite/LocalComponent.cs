using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Afrodite.Abstract;
using Afrodite.Concrete;

namespace Afrodite
{
    public class LocalComponent<T> : IComponent<T>, ITaskRunner<T>
    {
        private Guid machineGuid;
        private IPerformanceManager performanceManager;
        private List<IJob<T>> activeJobs;

        public LocalComponent(Guid machineGuid)
        {
            this.machineGuid = machineGuid;
            performanceManager = new PerformanceManager();
            activeJobs = new List<IJob<T>>();
        }

        public IComponentProperties Properties { get; private set; }

        public IComponentState<T> State
        {
            get { return CurrentState(); }
        }

        public Func<T, bool> TaskFunc { get; set; }

        public IComponentState<T> CurrentState()
        {
            return new ComponentState<T>(performanceManager.GetCpusUsage(), machineGuid,
                performanceManager.GetUsedMemory(), GetActiveJobs());
        }

        public IEnumerable<IJob<T>> GetActiveJobs()
        {
            return activeJobs;
        }

        public void PauseJob(IJob<T> job)
        {
            throw new NotImplementedException();
        }

        public void ResumeJob(IJob<T> job)
        {
            throw new NotImplementedException();
        }

        public bool RunTask(T task)
        {
            if (TaskFunc == null)
            {
                throw new InvalidOperationException("No taks runner");
            }
            return TaskFunc(task);
        }

        public IJob<T> StartJob(T job)
        {
            var task = Task.Factory.StartNew(() => RunTask(job));

            var runningJob = new Job<T>(new Guid()) {JobData = job, State = JobState.InProgress};
            activeJobs.Add(runningJob);
            return runningJob;
        }

        public bool TerminateJob(IJob<T> job)
        {
            throw new NotImplementedException();
        }
    }
}