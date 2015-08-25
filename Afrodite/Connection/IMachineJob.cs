using System;
using Apache.NMS;
using Apache.NMS.ActiveMQ.Commands;
using System.Diagnostics;

namespace Afrodite
{
	public interface IMachineJob<T>
	{
		Guid Guid { get; set; }
		DateTime StartTime { get; set; }
		T JobDetails {get;set;}
	}

}

