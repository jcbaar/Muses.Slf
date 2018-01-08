using System;

namespace Muses.Slf.Interfaces
{
    /// <summary>
    /// Logger interface which must be implemented by the log system implementation.
    /// </summary>
    public interface ILogger
    {
        void Log(Level level, Exception exception, string message, params object[] args);
        void Trace(string message, params object[] args);
        void Debug(string message, params object[] args);
        void Info(string message, params object[] args);
        void Warn(string message, params object[] args);
        void Error(string message, params object[] args);
        void Fatal(string message, params object[] args);
        void TraceException(Exception exception, string message, params object[] args);
        void DebugException(Exception exception, string message, params object[] args);
        void InfoException(Exception exception, string message, params object[] args);
        void WarnException(Exception exception, string message, params object[] args);
        void ErrorException(Exception exception, string message, params object[] args);
        void FatalException(Exception exception, string message, params object[] args);
    }
}
