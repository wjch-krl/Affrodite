using System;
using System.Collections.Generic;
using Afrodite.Abstract;
using Afrodite.Concrete;
using Afrodite.Connection;

namespace Afrodite
{
    public class RemoteComponent<T> : IComponent<T>, ITaskRunner<T>
    {
        private MasterRemoteEndpoint<T> remoteConnection;
        private Guid componentGuid;

        public RemoteComponent(MasterRemoteEndpoint<T> remoteConnection, Guid componentGuid)
        {
            this.remoteConnection = remoteConnection;
            this.componentGuid = componentGuid;
        }

        public IJob<T> StartJob(T job)
        {
            var newJob = new Job<T>(Guid.NewGuid()) {State = JobState.ReadyToStart, JobData = job};
            bool result = remoteConnection.StartNewJob(newJob, this.Properties.MachineId);
            return result ? newJob : null;
        }

        public void PauseJob(IJob<T> job)
        {
            throw new NotImplementedException();
        }

        public void ResumeJob(IJob<T> job)
        {
            throw new NotImplementedException();
        }

        public bool TerminateJob(IJob<T> job)
        {
            throw new NotImplementedException();
        }

        public IComponentProperties Properties { get { return remoteConnection.MachineManager[componentGuid]; } }
        public IComponentState<T> State { get { return remoteConnection.MachineStates[componentGuid].Last; } }
        public Func<T, bool> TaskFunc { get; set; }

        public IEnumerable<IJob<T>> GetActiveJobs()
        {
            throw new NotImplementedException();
        }

        public IComponentState<T> CurrentState()
        {
            throw new NotImplementedException();
        }
    }
}