using System;
using System.Collections.Generic;
using Afrodite.Abstract;
using Afrodite.Concrete;
using Afrodite.Connection;

namespace Afrodite
{
    public class RemoteComponent<TJob> : IComponent<TJob>
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
            remoteConnection.PauseJob(job.JobId, componentGuid);
        }

        public void ResumeJob(IJob<TJob> job)
        {
            remoteConnection.ResumeJob(job.JobId, componentGuid);
        }

        public bool TerminateJob(IJob<TJob> job)
        {
           return remoteConnection.StopJob(job.JobId, componentGuid);
        }

        public IComponentProperties Properties { get { return remoteConnection.MachineManager[componentGuid]; } }
        public IComponentState<TJob> State { get { return remoteConnection.MachineStates[componentGuid].Last; } }

        public IEnumerable<IJob<TJob>> GetActiveJobs()
        {
            throw new NotImplementedException();
        }
    }
}