using System.Collections.Generic;
using Afrodite.Abstract;
using Afrodite.Connection.Abstract;

namespace Afrodite.Connection.Concrete
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

