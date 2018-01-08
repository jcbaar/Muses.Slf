using Muses.Slf.Interfaces;
using NLog;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Muses.Slf.NLog
{
    /// <summary>
    /// NLog <see cref="ILoggerFactory"/> implementation.
    /// </summary>
    public class NLogLoggerFactory : BaseLoggerFactory, ILoggerFactory
    {
        static Dictionary<Level, LogLevel> _levelTable;
        static Dictionary<LogLevel, Level> _reverseLevelTable;

        #region Construction
        /// <summary>
        /// Static constructor. Set's up the static data and configures NLog.
        /// </summary>
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

            _reverseLevelTable = _levelTable.ToDictionary(x => x.Value, y => y.Key);

            ConfigurationItemFactory.Default.Targets.RegisterDefinition("Event", typeof(EventTarget));
            LogManager.ReconfigExistingLoggers();
        }

        /// <summary>
        /// Private constructor. This is a singleton class. Only
        /// one instance can exist.
        /// </summary>
        private NLogLoggerFactory()
        {
            Factory = this;
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Get's the singleton instance of this class.
        /// </summary>
        public static ILoggerFactory Factory { get; protected set; }

        /// <summary>
        /// Get's the name of the facade logging implementation.
        /// </summary>
        public string Name => "NLog";
        #endregion

        #region Public methods
        /// <summary>
        /// Converts a <see cref="Level"/> to a NLog <see cref="NLog.LogLevel"/>.
        /// </summary>
        /// <param name="level">The <see cref="Level"/> to convert.</param>
        /// <returns>The NLog <see cref="NLog.LogLevel"/>. If an unknown <see cref="Level"/> is used
        /// <see cref="NLog.LogLevel.Off"/> is returned.</returns>
        public static LogLevel ToNLogLevel(Level level)
        {
            if(_levelTable.TryGetValue(level, out LogLevel result))
            {
                return result;
            }
            return LogLevel.Off;
        }

        /// <summary>
        /// Converts a NLog <see cref="NLog.LogLevel"/> to a <see cref="Level"/>.
        /// </summary>
        /// <param name="level">The <see cref="NLog.LogLevel"/> to convert.</param>
        /// <returns>The <see cref="Level"/>. If an unknown <see cref="NLog.LogLevel"/> is used
        /// <see cref="Level.Other"/> is returned.</returns>
        public static Level ToLevel(LogLevel level)
        {
            if(_reverseLevelTable.TryGetValue(level, out Level result))
            {
                return result;
            }
            return Level.Other;
        }

        /// <summary>
        /// Get's the instance of the named <see cref="ILogger"/> instance.
        /// </summary>
        /// <param name="name">The name of the <see cref="ILogger"/> instance.</param>
        /// <returns>The instance of the <see cref="ILogger"/></returns>
        public Interfaces.ILogger GetLogger(string name) => GetOrAddLogger(name, (n) => new NLogLogger(n));

        /// <summary>
        /// Get's the instance of the named <see cref="ILogger"/> instance. The name used is the full
        /// name of the specified type.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to use as a name of the <see cref="ILogger"/> instance.</param>
        /// <returns>The instance of the <see cref="ILogger"/></returns>
        public Interfaces.ILogger GetLogger(Type type) => GetOrAddLogger(type.FullName, (n) => new NLogLogger(n));

        /// <summary>
        /// Registers a listener action for logging events. Whenever a logging event
        /// occurs that is configured to also be sent to the built-in event listener
        /// it will call the actions(s) registered here.
        /// </summary>
        /// <param name="listener">The <see cref="Action{LogEvent}"/> which must be called
        /// when a logging event occurred.</param>
        public void RegisterEventListener(Action<LogEvent> listener) => RegisterListener(listener);

        /// <summary>
        /// Unregisters a listener action for logging events previously registered with
        /// <see cref="RegisterListener(Action{LogEvent})"/>.
        /// </summary>
        /// <param name="listener">The <see cref="Action{LogEvent}"/> which must no longer be called
        /// when a logging event occurred.</param>
        public void UnregisterEventListener(Action<LogEvent> listener) => UnregisterListener(listener);

        /// <summary>
        /// Protected method that derived classes should use to call the registered
        /// <see cref="Action{LogEvent}"/> callback action(s).
        /// </summary>
        /// <param name="logEvent">The <see cref="LogEvent"/> containing the logging information.</param>
        public void RaiseEvent(LogEvent logEvent) => Raise(logEvent);

        /// <summary>
        /// Converts the instance to a string.
        /// </summary>
        /// <returns>The <see cref="Name"/> of the instance.</returns>
        public override string ToString() => Name;
        #endregion
    }
}
