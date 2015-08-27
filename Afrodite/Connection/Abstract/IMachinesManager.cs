using System;
using Afrodite.Abstract;

namespace Afrodite.Connection.Abstract
{
	public interface IMachinesManager
	{
		void Add(IComponentProperties props);
	    IComponentProperties this[Guid componentGuid] { get; }
	}

}

