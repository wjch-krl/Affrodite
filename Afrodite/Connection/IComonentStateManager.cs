using System;
using Apache.NMS;

namespace Afrodite
{
	public interface ICurrentMachineStateManager
	{
		 IComponentState CurrentState {get;}
	}

}

