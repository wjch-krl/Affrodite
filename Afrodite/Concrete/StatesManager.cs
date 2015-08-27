using System;
using System.Collections.Generic;
using Afrodite.Connection.Abstract;

namespace Afrodite.Concrete
{
	public class StatesManager<TJob> : IStatesManager<TJob>
	{
        private IDictionary<Guid, IMachineStateCollection<TJob>> states;
		private Func<IMachineStateCollection<TJob>> statesCollection;

		public IMachineStateCollection<TJob> this [Guid key]
		{
			get
			{
				if (!states.ContainsKey (key))
				{
					states.Add (key, statesCollection());
				}
				return states [key];		
			}
			set
			{
				states [key] = value;	
			}
		}


		public StatesManager (Func<IMachineStateCollection<TJob>> statesCollection)
		{
			this.statesCollection = statesCollection;
            states = new Dictionary<Guid, IMachineStateCollection<TJob>>();
		}
	}
}

