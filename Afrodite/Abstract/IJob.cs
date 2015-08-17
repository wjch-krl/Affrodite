
namespace Afrodite
{
	public interface IJob 
	{
		JobState State { get; set; }
		int JobId { get; }
	}
}