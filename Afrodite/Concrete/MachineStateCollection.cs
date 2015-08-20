using System;
using System.Collections.Generic;

namespace Afrodite
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

