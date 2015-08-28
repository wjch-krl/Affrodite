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

            return sf.GetMethod().Name;
        }
    }
}
