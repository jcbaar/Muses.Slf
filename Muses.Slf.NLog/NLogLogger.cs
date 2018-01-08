using NLog;
using System;

namespace Muses.Slf.NLog
{
    /// <summary>
    /// NLog based <see cref="ILogger"/> implementation.
    /// </summary>
    class NLogLogger : Muses.Slf.Interfaces.ILogger
    {
        private Logger _logger;

        /// <summary>
        /// Constructor. Initializes an instance of the object.
        /// </summary>
        /// <param name="name"></param>
        public NLogLogger(string name)
        {
            _logger = LogManager.GetLogger(name);
        }

        /// <summary>
        /// Executes a logging.
        /// </summary>
        /// <param name="level">The <see cref="Level"/> of the logging.</param>
        /// <param name="exception">Optionally an <see cref="Exception"/> object to accompany the logging.</param>
        /// <param name="message">The log message.</param>
        /// <param name="args">Any formatting arguments for the log message.</param>
        public void Log(Level level, Exception exception, string message, params object[] args)
        {
            var nlogLevel = NLogLoggerFactory.ToNLogLevel(level);
            if (_logger.IsEnabled(nlogLevel))
            {
                _logger.Log(nlogLevel, exception, message, args);
            }
        }

        /// <summary>
        /// Outputs a <see cref="Level.Trace"/> logging message.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="args">Any formatting arguments for the log message.</param>
        public void Trace(string message, params object[] args) => Log(Level.Trace, null, message, args);

        /// <summary>
        /// Outputs a <see cref="Level.Debug"/> logging message.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="args">Any formatting arguments for the log message.</param>
        public void Debug(string message, params object[] args) => Log(Level.Debug, null, message, args);

        /// <summary>
        /// Outputs a <see cref="Level.Info"/> logging message.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="args">Any formatting arguments for the log message.</param>
        public void Info(string message, params object[] args) => Log(Level.Info, null, message, args);

        /// <summary>
        /// Outputs a <see cref="Level.Warning"/> logging message.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="args">Any formatting arguments for the log message.</param>
        public void Warn(string message, params object[] args) => Log(Level.Warning, null, message, args);

        /// <summary>
        /// Outputs a <see cref="Level.Error"/> logging message.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="args">Any formatting arguments for the log message.</param>
        public void Error(string message, params object[] args) => Log(Level.Error, null, message, args);

        /// <summary>
        /// Outputs a <see cref="Level.Fatal"/> logging message.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="args">Any formatting arguments for the log message.</param>
        public void Fatal(string message, params object[] args) => Log(Level.Fatal, null, message, args);

        /// <summary>
        /// Outputs a <see cref="Level.Trace"/> logging message.
        /// </summary>
        /// <param name="exception">Optionally an <see cref="Exception"/> object to accompany the logging.</param>
        /// <param name="message">The log message.</param>
        /// <param name="args">Any formatting arguments for the log message.</param>
        public void TraceException(Exception exception, string message, params object[] args) => Log(Level.Trace, exception, message, args);

        /// <summary>
        /// Outputs a <see cref="Level.Debug"/> logging message.
        /// </summary>
        /// <param name="exception">Optionally an <see cref="Exception"/> object to accompany the logging.</param>
        /// <param name="message">The log message.</param>
        /// <param name="args">Any formatting arguments for the log message.</param>
        public void DebugException(Exception exception, string message, params object[] args) => Log(Level.Debug, exception, message, args);

        /// <summary>
        /// Outputs a <see cref="Level.Info"/> logging message.
        /// </summary>
        /// <param name="exception">Optionally an <see cref="Exception"/> object to accompany the logging.</param>
        /// <param name="message">The log message.</param>
        /// <param name="args">Any formatting arguments for the log message.</param>
        public void InfoException(Exception exception, string message, params object[] args) => Log(Level.Info, exception, message, args);

        /// <summary>
        /// Outputs a <see cref="Level.Warning"/> logging message.
        /// </summary>
        /// <param name="exception">Optionally an <see cref="Exception"/> object to accompany the logging.</param>
        /// <param name="message">The log message.</param>
        /// <param name="args">Any formatting arguments for the log message.</param>
        public void WarnException(Exception exception, string message, params object[] args) => Log(Level.Warning, exception, message, args);

        /// <summary>
        /// Outputs a <see cref="Level.Error"/> logging message.
        /// </summary>
        /// <param name="exception">Optionally an <see cref="Exception"/> object to accompany the logging.</param>
        /// <param name="message">The log message.</param>
        /// <param name="args">Any formatting arguments for the log message.</param>
        public void ErrorException(Exception exception, string message, params object[] args) => Log(Level.Error, exception, message, args);

        /// <summary>
        /// Outputs a <see cref="Level.Fatal"/> logging message.
        /// </summary>
        /// <param name="exception">Optionally an <see cref="Exception"/> object to accompany the logging.</param>
        /// <param name="message">The log message.</param>
        /// <param name="args">Any formatting arguments for the log message.</param>
        public void FatalException(Exception exception, string message, params object[] args) => Log(Level.Fatal, exception, message, args);
    }
}
