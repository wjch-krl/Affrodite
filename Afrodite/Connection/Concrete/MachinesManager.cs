using System;
using System.Collections.Generic;

namespace Afrodite
{
	public class MachinesManager : IMachinesManager
	{
		private IDictionary<int,IComponentProperties> list;
		public void Add (IComponentProperties props)
		{
			if (list.ContainsKey (props.MachineId))
			{
				list [props.MachineId] = props;
			}
			else
			{
				list.Add (props.MachineId, props);
			}
		}
			
		public MachinesManager ()
		{
			this.list = new Dictionary<int, IComponentProperties> ();
		}
	}
}

