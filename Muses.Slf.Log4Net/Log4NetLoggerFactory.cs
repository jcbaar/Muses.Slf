using Muses.Slf.Interfaces;
using System;
using System.Collections.Generic;

namespace Muses.Slf.Log4Net
{
    public class Log4NetLoggerFactory : BaseLoggerFactory, ILoggerFactory
    {
        static Dictionary<Level, log4net.Core.Level> _levelTable;
        static Dictionary<log4net.Core.Level, Level> _reverseLevelTable;

        static Log4NetLoggerFactory()
        {
            log4net.Config.XmlConfigurator.Configure();
            _levelTable = new Dictionary<Level, log4net.Core.Level>()
            {
                {Level.Trace, log4net.Core.Level.Trace},
                {Level.Debug, log4net.Core.Level.Debug},
                {Level.Info, log4net.Core.Level.Info},
                {Level.Warning, log4net.Core.Level.Warn},
                {Level.Error, log4net.Core.Level.Error},
                {Level.Fatal, log4net.Core.Level.Fatal}
            };

            _reverseLevelTable = new Dictionary<log4net.Core.Level, Level>()
            {
                {log4net.Core.Level.Trace, Level.Trace},
                {log4net.Core.Level.Debug, Level.Debug},
                {log4net.Core.Level.Info,  Level.Info},
                {log4net.Core.Level.Warn,  Level.Warning},
                {log4net.Core.Level.Error, Level.Error},
                {log4net.Core.Level.Fatal, Level.Fatal}
            };
        }

        public static Log4NetLoggerFactory Factory;
        private Log4NetLoggerFactory()
        {
            Factory = this;
        }

        public static log4net.Core.Level ToLog4NetLevel(Level level)
        {
            if(_levelTable.ContainsKey(level))
            {
                return _levelTable[level];
            }
            return log4net.Core.Level.Off;
        }

        public static Level ToLevel(log4net.Core.Level level)
        {
            if (_reverseLevelTable.ContainsKey(level))
            {
                return _reverseLevelTable[level];
            }
            return Level.Other;
        }

        public string Name => "log4net";

        public ILogger GetLogger(string name) => new Log4NetLogger(name);

        public ILogger GetLogger(Type type) => new Log4NetLogger(type.FullName);

        public void RegisterEventListener(Action<LogEvent> listener) => RegisterListener(listener);

        public void UnregisterEventListener(Action<LogEvent> listener) => UnregisterListener(listener);

        internal void RaiseEvent(LogEvent logEvent) => Raise(logEvent);

        public override string ToString() => Name;
    }
}
