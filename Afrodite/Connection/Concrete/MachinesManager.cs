using System;
using System.Collections.Generic;
using Afrodite.Abstract;
using Afrodite.Connection.Abstract;

namespace Afrodite.Connection.Concrete
{
	public class MachinesManager : IMachinesManager
	{
		private IDictionary<Guid,IComponentProperties> list;
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

	    public IComponentProperties this[Guid componentGuid]
	    {
	        get { return list[componentGuid]; }
	    }

	    public MachinesManager ()
		{
			this.list = new Dictionary<Guid, IComponentProperties> ();
		}
	}
}

