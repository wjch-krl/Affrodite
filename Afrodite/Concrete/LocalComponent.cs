using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Afrodite.Abstract;

namespace Afrodite.Concrete
{
    public class LocalComponent<TJob> : IComponent<TJob>, ITaskRunner<TJob>
    {
        private Guid machineGuid;
        private IPerformanceManager performanceManager;
        private List<IJob<TJob>> activeJobs;

        public LocalComponent(Guid machineGuid)
        {
            this.machineGuid = machineGuid;
            performanceManager = new PerformanceManager();
            activeJobs = new List<IJob<TJob>>();
        }

        public IComponentProperties Properties { get; private set; }

        public IComponentState<TJob> State
        {
            get { return CurrentState(); }
        }

        public Func<TJob, bool> StartTaskAction { get; set; }
        public Func<TJob, bool> StopTaskAction { get; set; }
        public Func<TJob, bool> PauseTaskAction { get; set; }
        public Func<TJob, bool> ResumeTaskAction { get; set; }

        public IComponentState<TJob> CurrentState()
        {
            return new ComponentState<TJob>(performanceManager.GetCpusUsage(), machineGuid,
                performanceManager.GetUsedMemory(), GetActiveJobs());
        }

        public IEnumerable<IJob<TJob>> GetActiveJobs()
        {
            return activeJobs;
        }

        public void PauseJob(IJob<TJob> job)
        {
            PauseTaskAction(job.JobData);
        }

        public void ResumeJob(IJob<TJob> job)
        {
            ResumeTaskAction(job.JobData);
        }

        public bool RunTask(TJob task)
        {
            if (StartTaskAction == null)
            {
                throw new InvalidOperationException("No taks runner");
            }
            return StartTaskAction(task);
        }

        public IJob<TJob> StartJob(TJob job)
        {
            Task.Factory.StartNew(() => RunTask(job));
            var runningJob = new Job<TJob>(new Guid()) {JobData = job, State = JobState.InProgress};
            activeJobs.Add(runningJob);
            return runningJob;
        }

        public bool TerminateJob(IJob<TJob> job)
        {
            return StopTaskAction(job.JobData);
        }
    }
}