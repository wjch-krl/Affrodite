using System;
using Apache.NMS;
using Apache.NMS.ActiveMQ.Commands;
using System.Diagnostics;

namespace Afrodite
{
	public interface IStatesManager
	{
		IMachineStateCollection this [int key]
		{
			get;
			set;
		}
	}

}

