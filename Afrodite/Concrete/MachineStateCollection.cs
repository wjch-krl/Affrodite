using System.Collections.Generic;
using Afrodite.Abstract;
using Afrodite.Connection.Abstract;

namespace Afrodite.Concrete
{
	public class MachineStateCollection : IMachineStateCollection
	{
		private Queue<IComponentState> list;
		private int capcity;

		public void Add (IComponentState state)
		{
			if (list.Count == capcity)
			{
				list.Dequeue ();
			}
			list.Enqueue (state);
		}

		public MachineStateCollection (int capcity = 10)
		{
			this.capcity = capcity;
			this.list = new Queue<IComponentState> (10);
		}
	}
}

