using Muses.Slf.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muses.Slf.Tests
{
    [ExcludeFromCodeCoverage]
    class TestFactory : BaseLoggerFactory, ILoggerFactory
    {
        public string Name => "TestFactory";

        public ILogger GetLogger(string name)
        {
            return new NopLogger();
        }

        public ILogger GetLogger(Type type)
        {
            return new NopLogger();
        }

        public bool RaiseEvent(LogEvent logEvent)
        {
            return Raise(logEvent);
        }

        public bool RegisterEventListener(Action<LogEvent> listener)
        {
            return RegisterListener(listener);
        }

        public bool UnregisterEventListener(Action<LogEvent> listener)
        {
            return base.UnregisterListener(listener);
        }
    }
}
