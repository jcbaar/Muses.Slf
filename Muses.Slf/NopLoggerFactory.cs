using Muses.Slf.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Muses.Slf
{
    /// <summary>
    /// A "No-operation" logger factory. This is a placeholder logger factory used when no concrete
    /// implementation of the logging facade was found. It simply does nothing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class NopLoggerFactory : ILoggerFactory
    {
        private NopLogger _nop = new NopLogger();

        public NopLoggerFactory Factory;
        internal NopLoggerFactory()
        {
            Factory = this;
        }

        public string Name => "NOP";

        public Interfaces.ILogger GetLogger(string name) => _nop;

        public Interfaces.ILogger GetLogger(Type type) => _nop;

        public bool RegisterEventListener(Action<LogEvent> listener) { return false; }

        public bool UnregisterEventListener(Action<LogEvent> listener) { return false; }

        public bool RaiseEvent(LogEvent logEvent) { return false; }

        public override string ToString() => Name;
    }
}