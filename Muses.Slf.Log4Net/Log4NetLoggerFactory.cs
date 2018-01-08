using Muses.Slf.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Muses.Slf.Log4Net
{
    /// <summary>
    /// log4net <see cref="ILoggerFactory"/> implementation.
    /// </summary>
    public class Log4NetLoggerFactory : BaseLoggerFactory, ILoggerFactory
    {
        static Dictionary<Level, log4net.Core.Level> _levelTable;
        static Dictionary<log4net.Core.Level, Level> _reverseLevelTable;

        #region Construction
        /// <summary>
        /// Static constructor. Set's up the static data and configures log4net.
        /// </summary>
        static Log4NetLoggerFactory()
        {
            _levelTable = new Dictionary<Level, log4net.Core.Level>()
            {
                {Level.Trace, log4net.Core.Level.Trace},
                {Level.Debug, log4net.Core.Level.Debug},
                {Level.Info, log4net.Core.Level.Info},
                {Level.Warning, log4net.Core.Level.Warn},
                {Level.Error, log4net.Core.Level.Error},
                {Level.Fatal, log4net.Core.Level.Fatal}
            };

            _reverseLevelTable = _levelTable.ToDictionary(x => x.Value, y => y.Key);

            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Private constructor. This is a singleton class. Only
        /// one instance can exist.
        /// </summary>
        private Log4NetLoggerFactory()
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
        public string Name => "log4net";
        #endregion

        #region Public methods
        /// <summary>
        /// Converts a <see cref="Level"/> to a log4net <see cref="log4net.Core.Level"/>.
        /// </summary>
        /// <param name="level">The <see cref="Level"/> to convert.</param>
        /// <returns>The log4net <see cref="log4net.Core.Level"/>. If an unknown <see cref="Level"/> is used
        /// <see cref="log4net.Core.Level.Off"/> is returned.</returns>
        public static log4net.Core.Level ToLog4NetLevel(Level level)
        {
            if(_levelTable.TryGetValue(level, out log4net.Core.Level result))
            {
                return result;
            }
            return log4net.Core.Level.Off;
        }

        /// <summary>
        /// Converts a log4net <see cref="log4net.Core.Level"/> to a <see cref="Level"/>.
        /// </summary>
        /// <param name="level">The <see cref="log4net.Core.Level"/> to convert.</param>
        /// <returns>The <see cref="Level"/>. If an unknown <see cref="log4net.Core.Level"/> is used
        /// <see cref="Level.Other"/> is returned.</returns>
        public static Level ToLevel(log4net.Core.Level level)
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
        public ILogger GetLogger(string name) => GetOrAddLogger(name, (n) => new Log4NetLogger(n));

        /// <summary>
        /// Get's the instance of the named <see cref="ILogger"/> instance. The name used is the full
        /// name of the specified type.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to use as a name of the <see cref="ILogger"/> instance.</param>
        /// <returns>The instance of the <see cref="ILogger"/></returns>
        public ILogger GetLogger(Type type) => GetOrAddLogger(type.FullName, (n) => new Log4NetLogger(n));

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
