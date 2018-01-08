using System;
using System.Diagnostics.CodeAnalysis;

namespace Muses.Slf
{
    /// <summary>
    /// Simple class holding the relevant log information.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LogEvent
    {
        /// <summary>
        /// The <see cref="Level"/> of the logging.
        /// </summary>
        public Level LogLevel { get; set; }

        /// <summary>
        /// The rendered (formatted) log message.
        /// </summary>
        public string RenderedMessage { get; set; }

        /// <summary>
        /// The date and time of the logging.
        /// </summary>
        public DateTime Stamp { get; set; }

        /// <summary>
        /// The exception accompanying the logging. 
        /// </summary>
        public Exception Exception { get; set; }
    }
}
