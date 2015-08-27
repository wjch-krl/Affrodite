using System;

namespace Afrodite.Abstract
{
    public class Component<T> : IComponent<T>
    {
        public IJob<T> StartJob(T job)
        {
            throw new NotImplementedException();
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

        public IComponentProperties Properties { get; private set; }
        public IComponentState<T> State { get; private set; }
    }
}