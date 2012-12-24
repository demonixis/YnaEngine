using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yna.Framework.Helpers
{
    /// <summary>
    /// A basic helper for debug
    /// </summary>
    public class DebugHelper
    {
        /// <summary>
        /// Show an error message in console
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
#if !NETFX_CORE
            Console.Error.WriteLine("[ERROR] {0}", message);
#else

#endif
        }

        /// <summary>
        /// Show a trace message in console
        /// </summary>
        /// <param name="message"></param>
        public static void Trace(string message)
        {
#if !NETFX_CORE
            Console.WriteLine("[TRACE] {0}", message);
#else

#endif
        }
    }
}
