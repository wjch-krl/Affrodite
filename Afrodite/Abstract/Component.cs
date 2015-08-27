using System;

namespace Afrodite.Abstract
{
    public class Component<TJob> : IComponent<TJob>
    {
        public IJob<TJob> StartJob(TJob job)
        {
            throw new NotImplementedException();
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

        public IComponentProperties Properties { get; private set; }
        public IComponentState<TJob> State { get; private set; }
    }
}