using log4net;
using Muses.Slf.Interfaces;
using System;

namespace Muses.Slf.Log4Net
{
    class Log4NetLogger : ILogger
    {
        private ILog _logger;

        public Log4NetLogger(string name)
        {
            _logger = LogManager.GetLogger(name);
        }

        public void Log(Level level, Exception exception, string message, params object[] args)
        {
            var l4nLevel = Log4NetLoggerFactory.ToLog4NetLevel(level);
            if (_logger.Logger.IsEnabledFor(l4nLevel))
            {
                _logger.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                    l4nLevel, String.Format(message, args), exception);
            }
        }

        public void Trace(string message, params object[] args) => Log(Level.Trace, null, message, args);

        public void Debug(string message, params object[] args) => Log(Level.Debug, null, message, args);

        public void Error(string message, params object[] args) => Log(Level.Error, null, message, args);

        public void Info(string message, params object[] args) => Log(Level.Info, null, message, args);

        public void Warn(string message, params object[] args) => Log(Level.Warning, null, message, args);

        public void Fatal(string message, params object[] args) => Log(Level.Fatal, null, message, args);

        public void TraceException(Exception exception, string message, params object[] args) => Log(Level.Trace, exception, message, args);

        public void DebugException(Exception exception, string message, params object[] args) => Log(Level.Debug, exception, message, args);

        public void ErrorException(Exception exception, string message, params object[] args) => Log(Level.Error, exception, message, args);

        public void InfoException(Exception exception, string message, params object[] args) => Log(Level.Info, exception, message, args);

        public void WarnException(Exception exception, string message, params object[] args) => Log(Level.Warning, exception, message, args);

        public void FatalException(Exception exception, string message, params object[] args) => Log(Level.Fatal, exception, message, args);

    }
}
