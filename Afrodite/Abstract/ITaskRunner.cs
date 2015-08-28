using System;
using System.Collections.Generic;

namespace Afrodite.Abstract
{
    public interface ITaskRunner<TJob>
    {
        Func<TJob, bool> StartTaskAction { get; set; }
        Func<TJob, bool> StopTaskAction { get; set; }
        Func<TJob, bool> PauseTaskAction { get; set; }
        Func<TJob, bool> ResumeTaskAction { get; set; }

        IEnumerable<IJob<TJob>> GetActiveJobs();
        IComponentState<TJob> CurrentState();
    }
}