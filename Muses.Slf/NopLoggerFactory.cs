using Muses.Slf.Interfaces;
using System;

namespace Muses.Slf
{
    /// <summary>
    /// A "No-operation" logger factory. This is a placeholder logger factory used when no concrete
    /// implementation of the logging facade was found. It simply does nothing.
    /// </summary>
    public class NopLoggerFactory : ILoggerFactory
    {
        private NopLogger _nop = new NopLogger();

        public NopLoggerFactory Factory = new NopLoggerFactory();
        internal NopLoggerFactory() { }

        public string Name => "NOP";

        public Interfaces.ILogger GetLogger(string name) => _nop;

        public Interfaces.ILogger GetLogger(Type type) => _nop;

        public void RegisterEventListener(Action<LogEvent> listener) { }

        public void UnregisterEventListener(Action<LogEvent> listener) { }

        internal void RaiseEvent(LogEvent logEvent) { }

        public override string ToString() => Name;
    }
}