using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Afrodite.Common
{
    public static class Logger
    {
        public static void LoggError(Exception ex, string message)
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            var method = sf.GetMethod();
            if (method.DeclaringType != null)
                return string.Format("{0}.{1}", method.DeclaringType.FullName, method.Name);
            return method.Name;
        }
    }
}