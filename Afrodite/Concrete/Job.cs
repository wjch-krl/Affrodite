using System;
using Afrodite.Abstract;

namespace Afrodite.Concrete
{
    public class Job<T> : IJob<T>
    {
        public Job(Guid jobId)
        {
            JobId = jobId;
        }

        public JobState State { get; set; }
        public Guid JobId { get; private set; }
        public T JobData { get; set; }
    }
}