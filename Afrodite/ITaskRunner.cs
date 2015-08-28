using System;
using System.Collections.Generic;
using Afrodite.Abstract;

namespace Afrodite
{
    public interface ITaskRunner<TJob>
    {
        Func<TJob, bool> StartTaskAction { get; set; }
        Func<TJob, bool> StopTaskAction { get; set; }
        Func<TJob, bool> PauseTaskAction { get; set; }
        Func<TJob, bool> ResumeTaskAction { get; set; }
    }
}