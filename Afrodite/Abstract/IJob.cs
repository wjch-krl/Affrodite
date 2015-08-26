
using System;

namespace Afrodite.Abstract
{
	public interface IJob <T>
	{
		JobState State { get; set; }
		Guid JobId { get; }
        T JobData { get; set; }
	}
}