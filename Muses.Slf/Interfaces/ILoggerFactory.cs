using System;

namespace Muses.Slf.Interfaces
{
    /// <summary>
    /// Factory interface which must be implemented by the log system implementation.
    /// </summary>
    public interface ILoggerFactory
    {
        string Name { get; }

        ILogger GetLogger(string name);

        ILogger GetLogger(Type type);

        void RegisterEventListener(Action<LogEvent> listener);

        void UnregisterEventListener(Action<LogEvent> listener);

        void RaiseEvent(LogEvent logEvent);
    }
}
