using Muses.Slf.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Muses.Slf
{
    /// <summary>
    /// Base class for a logging factory implementation. All implementations of a
    /// <see cref="Interfaces.ILoggingFactory"/> class should also derive from this
    /// base class because it implements the basics for the logging events.
    /// </summary>
    public class BaseLoggerFactory
    {
        private ReaderWriterLockSlim _locker = new ReaderWriterLockSlim();
        private List<Action<LogEvent>> _listeners = new List<Action<LogEvent>>();
        private ConcurrentDictionary<String, ILogger> _loggers = new ConcurrentDictionary<string, ILogger>();

        /// <summary>
        /// Get's the named logger instance or creates a new one.
        /// </summary>
        /// <param name="name">The name of the <see cref="ILogger"/> instance.</param>
        /// <param name="factory">The factory function to create a new <see cref="ILogger"/> instance.</param>
        /// <returns>The named <see cref="ILogger"/> instance.</returns>
        [ExcludeFromCodeCoverage]
        protected ILogger GetOrAddLogger(string name, Func<string, ILogger> factory) => _loggers.GetOrAdd(name, factory);

        /// <summary>
        /// Registers a listener action for logging events. Whenever a logging event
        /// occurs that is configured to also be sent to the built-in event listener
        /// it will call the actions(s) registered here.
        /// </summary>
        /// <param name="listener">The <see cref="Action{LogEvent}"/> which must be called
        /// when a logging event occurred.</param>
        /// <returns>true if the listener was added, false if the listener was already added.</returns>
        protected bool RegisterListener(Action<LogEvent> listener)
        {
            try
            {
                _locker.EnterWriteLock();
                if (!_listeners.Contains(listener))
                {
                    _listeners.Add(listener);
                    return true;
                }
                return false;
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        /// <summary>
        /// Unregisters a listener action for logging events previously registered with
        /// <see cref="RegisterListener(Action{LogEvent})"/>.
        /// </summary>
        /// <param name="listener">The <see cref="Action{LogEvent}"/> which must no longer be called
        /// when a logging event occurred.</param>
        /// <returns>true if the listener was actually removed, false if the listener was not found.</returns>
        protected bool UnregisterListener(Action<LogEvent> listener)
        {
            try
            {
                _locker.EnterWriteLock();
                if (_listeners.Contains(listener))
                {
                    _listeners.Remove(listener);
                    return true;
                }
                return false;
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        /// <summary>
        /// Protected method that derived classes should use to call the registered
        /// <see cref="Action{LogEvent}"/> callback action(s).
        /// </summary>
        /// <param name="logEvent">The <see cref="LogEvent"/> containing the logging information.</param>
        /// <returns>true if at least one callback was called, false if no callback was called.</returns>
        protected bool Raise(LogEvent logEvent)
        {
            try
            {
                _locker.EnterReadLock();
                if (_listeners.Count == 0)
                {
                    return false;
                }
                else
                {
                    foreach (var action in _listeners)
                    {
                        action(logEvent);
                    }
                    return true;
                }
            }
            finally
            {
                _locker.ExitReadLock();
            }
        }
    }
}
