namespace Afrodite.Abstract
{
    public class Component<T> : IComponent<T>
    {


        public IJob<T> StartJob(T job)
        {
            throw new System.NotImplementedException();
        }

        public void PauseJob(IJob<T> job)
        {
            throw new System.NotImplementedException();
        }

        public void ResumeJob(IJob<T> job)
        {
            throw new System.NotImplementedException();
        }

        public bool TerminateJob(IJob<T> job)
        {
            throw new System.NotImplementedException();
        }

        public IComponentProperties Properties { get; private set; }
        public IComponentState<T> State { get; private set; }
    }
}