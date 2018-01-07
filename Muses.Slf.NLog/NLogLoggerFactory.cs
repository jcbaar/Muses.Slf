
using Muses.Slf.Interfaces;
using NLog;
using NLog.Config;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Muses.Slf.NLog
{
    public class NLogLoggerFactory : BaseLoggerFactory, ILoggerFactory
    {
        static Dictionary<Level, LogLevel> _levelTable;
        static Dictionary<LogLevel, Level> _reverseLevelTable;
        static NLogLoggerFactory()
        {
            _levelTable = new Dictionary<Level, LogLevel>()
            {
                {Level.Trace, LogLevel.Trace },
                {Level.Debug, LogLevel.Debug},
                {Level.Info, LogLevel.Info},
                {Level.Warning, LogLevel.Warn},
                {Level.Error, LogLevel.Error},
                {Level.Fatal, LogLevel.Fatal},
            };

            _reverseLevelTable = new Dictionary<LogLevel, Level>()
            {
                {LogLevel.Trace, Level.Trace },
                {LogLevel.Debug, Level.Debug},
                {LogLevel.Info, Level.Info},
                {LogLevel.Warn, Level.Warning},
                {LogLevel.Error, Level.Error},
                {LogLevel.Fatal, Level.Fatal},
            };

            ConfigurationItemFactory.Default.Targets.RegisterDefinition("Event", typeof(EventTarget));
            LogManager.ReconfigExistingLoggers();
        }

        public static LogLevel ToNLogLevel(Level level)
        {
            if (_levelTable.ContainsKey(level))
            {
                return _levelTable[level];
            }
            return LogLevel.Off;
        }

        public static Level ToLevel(LogLevel level)
        {
            if (_reverseLevelTable.ContainsKey(level))
            {
                return _reverseLevelTable[level];
            }
            return Level.Other;
        }

        public static NLogLoggerFactory Factory;
        private NLogLoggerFactory()
        {
            Factory = this;
        }

        public string Name => "NLog";

        public Interfaces.ILogger GetLogger(string name) => new NLogLogger(name);

        public Interfaces.ILogger GetLogger(Type type) => new NLogLogger(type.FullName);

        public void RegisterEventListener(Action<LogEvent> listener) => RegisterListener(listener);

        public void UnregisterEventListener(Action<LogEvent> listener) => UnregisterListener(listener);

        internal void RaiseEvent(LogEvent logEvent) => Raise(logEvent);

        public override string ToString() => Name;
    }
}
