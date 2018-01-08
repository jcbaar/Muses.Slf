using log4net;
using Muses.Slf.Interfaces;
using System;

namespace Muses.Slf.Log4Net
{
    /// <summary>
    /// log4net based <see cref="ILogger"/> implementation.
    /// </summary>
    class Log4NetLogger : ILogger
    {
        private ILog _logger;

        /// <summary>
        /// Constructor. Initializes an instance of the object.
        /// </summary>
        /// <param name="name"></param>
        public Log4NetLogger(string name)
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
            var l4nLevel = Log4NetLoggerFactory.ToLog4NetLevel(level);
            if (_logger.Logger.IsEnabledFor(l4nLevel))
            {
                _logger.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                    l4nLevel, String.Format(message, args), exception);
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
