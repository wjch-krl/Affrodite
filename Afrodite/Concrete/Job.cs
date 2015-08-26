using Afrodite.Abstract;

namespace Afrodite.Concrete
{
    public class Job : IJob
    {
        public Job(int jobId)
        {
            JobId = jobId;
        }
        public JobState State { get; set; }
        public int JobId { get; private set; }
    }
}