using System;
using System.Collections.Generic;

namespace Afrodite
{
	public class StatesManager : IStatesManager
	{
		private IDictionary<int,IMachineStateCollection> states;
		private Func<IMachineStateCollection> statesCollection;

		public IMachineStateCollection this [int key]
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


		public StatesManager (Func<IMachineStateCollection> statesCollection)
		{
			this.statesCollection = statesCollection;
			states = new Dictionary<int,IMachineStateCollection> ();
		}
	}
}

