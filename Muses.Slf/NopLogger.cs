using Muses.Slf.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Muses.Slf.Tests")]

namespace Muses.Slf
{
    /// <summary>
    /// A "No-operation" logger. This is a placeholder logger used when no concrete
    /// implementation of the logging facade was found. It simply does nothing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    class NopLogger : ILogger
    {
        public void Log(Level level, Exception exception, string message, params object[] args) { }

        public void Trace(string message, params object[] args) { }

        public void Debug(string message, params object[] args) { }

        public void Info(string message, params object[] args) { }

        public void Warn(string message, params object[] args) { }

        public void Error(string message, params object[] args) { }

        public void Fatal(string message, params object[] args) { }

        public void TraceException(Exception exception, string message, params object[] args) { }

        public void DebugException(Exception exception, string message, params object[] args) { }

        public void InfoException(Exception exception, string message, params object[] args) { }

        public void WarnException(Exception exception, string message, params object[] args) { }

        public void ErrorException(Exception exception, string message, params object[] args) { }

        public void FatalException(Exception exception, string message, params object[] args) { }
    }
}
