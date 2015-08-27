using System;
using System.Collections.Generic;
using Afrodite.Abstract;

namespace Afrodite
{
    public interface ITaskRunner<T>
    {
        Func<T, bool> TaskFunc { get; set; }
        IEnumerable<IJob<T>> GetActiveJobs();
        IComponentState<T> CurrentState();
    }
}