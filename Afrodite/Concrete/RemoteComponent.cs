using System;
using System.Collections.Generic;
using Afrodite.Abstract;
using Afrodite.Connection;

namespace Afrodite.Concrete
{
    public class RemoteComponent<TJob> : IComponent<TJob>, ITaskRunner<TJob>
    {
        private MasterRemoteEndpoint<TJob> remoteConnection;
        private Guid componentGuid;

        public RemoteComponent(MasterRemoteEndpoint<TJob> remoteConnection, Guid componentGuid)
        {
            this.remoteConnection = remoteConnection;
            this.componentGuid = componentGuid;
        }

        public IJob<TJob> StartJob(TJob job)
        {
            var newJob = new Job<TJob>(Guid.NewGuid()) {State = JobState.ReadyToStart, JobData = job};
            bool result = remoteConnection.StartNewJob(newJob, this.Properties.MachineId);
            return result ? newJob : null;
        }

        public void PauseJob(IJob<TJob> job)
        {
            throw new NotImplementedException();
        }

        public void ResumeJob(IJob<TJob> job)
        {
            throw new NotImplementedException();
        }

        public bool TerminateJob(IJob<TJob> job)
        {
            throw new NotImplementedException();
        }

        public IComponentProperties Properties { get { return remoteConnection.MachineManager[componentGuid]; } }
        public IComponentState<TJob> State { get { return remoteConnection.MachineStates[componentGuid].Last; } }
        public Func<TJob, bool> StartTaskAction { get; set; }
        public Func<TJob, bool> StopTaskAction { get; set; }
        public Func<TJob, bool> PauseTaskAction { get; set; }
        public Func<TJob, bool> ResumeTaskAction { get; set; }

        public IEnumerable<IJob<TJob>> GetActiveJobs()
        {
            throw new NotImplementedException();
        }

        public IComponentState<TJob> CurrentState()
        {
            throw new NotImplementedException();
        }
    }
}