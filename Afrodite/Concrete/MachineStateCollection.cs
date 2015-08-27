using System.Collections.Generic;
using System.Linq;
using Afrodite.Abstract;
using Afrodite.Connection.Abstract;

namespace Afrodite.Concrete
{
    public class MachineStateCollection<TJob> : IMachineStateCollection<TJob>
	{
        private Queue<IComponentState<TJob>> list;
		private int capcity;

        public void Add(IComponentState<TJob> state)
		{
			if (list.Count == capcity)
			{
				list.Dequeue ();
			}
			list.Enqueue (state);
		}

        public IComponentState<TJob> Last { get { return list.Last(); } }

        public MachineStateCollection (int capcity = 10)
		{
			this.capcity = capcity;
			this.list = new Queue<IComponentState<TJob>> (10);
		}
    }
}

