using System;
using System.Xml.Serialization;
using Afrodite.Abstract;

namespace Afrodite.Concrete
{
    public class Job<TJob> : IJob<TJob>
    {
        public Job(Guid jobId)
        {
            JobId = jobId;
        }

        public JobState State { get; set; }
        public Guid JobId { get; private set; }
        public TJob JobData { get; set; }
    }
}