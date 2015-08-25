using System;

namespace Afrodite
{
	public class Dereflektor
	{
		public T Create<T> (string pattern)
		{
			Type t = Type.GetType (pattern);
			if (t == null)
			{
				throw new InvalidOperationException ("Type " + pattern + " not found.");
			}
			return (T)Activator.CreateInstance (t);
		}
	}
}

