using System;

namespace Afrodite.Common
{
	public static class Deflector
	{
		public static TJob Create<TJob> (string pattern)
		{
			Type type = Type.GetType (pattern);
            if (type == null)
			{
				throw new InvalidOperationException ("Type " + pattern + " not found.");
			}
            return (TJob)Activator.CreateInstance(type);
		}
	}
}

