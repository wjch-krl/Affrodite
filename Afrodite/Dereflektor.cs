using System;

namespace Afrodite
{
	public static class Dereflektor
	{
		public static T Create<T> (string pattern)
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

