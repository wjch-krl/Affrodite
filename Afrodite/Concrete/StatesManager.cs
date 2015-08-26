﻿using System;
using System.Collections.Generic;
using Afrodite.Connection.Abstract;

namespace Afrodite.Concrete
{
	public class StatesManager<T> : IStatesManager<T>
	{
		private IDictionary<int,IMachineStateCollection<T>> states;
		private Func<IMachineStateCollection<T>> statesCollection;

		public IMachineStateCollection<T> this [int key]
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


		public StatesManager (Func<IMachineStateCollection<T>> statesCollection)
		{
			this.statesCollection = statesCollection;
			states = new Dictionary<int,IMachineStateCollection<T>> ();
		}
	}
}

