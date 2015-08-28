
using System;
using Afrodite.Concrete;

namespace Afrodite.Abstract
{
	public interface IJob <TJob>
	{
		JobState State { get; set; }
		Guid JobId { get; }
        TJob JobData { get; set; }
	}
}