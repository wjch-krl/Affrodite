using System.Collections.Generic;
using System.Linq;
using Afrodite.Abstract;
using Afrodite.Connection.Abstract;

namespace Afrodite.Concrete
{
    public class MachineStateCollection<T> : IMachineStateCollection<T>
	{
        private Queue<IComponentState<T>> list;
		private int capcity;

        public void Add(IComponentState<T> state)
		{
			if (list.Count == capcity)
			{
				list.Dequeue ();
			}
			list.Enqueue (state);
		}

        public IComponentState<T> Last { get { return list.Last(); } }

        public MachineStateCollection (int capcity = 10)
		{
			this.capcity = capcity;
			this.list = new Queue<IComponentState<T>> (10);
		}
    }
}

