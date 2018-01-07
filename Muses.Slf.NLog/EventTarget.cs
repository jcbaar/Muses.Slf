using NLog;
using NLog.Targets;
using System;

namespace Muses.Slf.NLog
{
    /// <summary>
    /// A NLog <see cref="Target"/> which will route logging events to the registered <see cref="Muses.Slf.Interfaces.ILoggerFactory"/>
    /// callback actions.
    /// </summary>
    [Target("Event")]
    public sealed class EventTarget : TargetWithLayout
    {
        /// <summary>
        /// Called when a logging was directed to this target.
        /// </summary>
        /// <param name="logEvent">The <see cref="LogEventInfo"/> describing the logging.</param>
        protected override void Write(LogEventInfo logEvent)
        {
            NLogLoggerFactory.Factory.RaiseEvent(new LogEvent
            {
                Exception = logEvent.Exception,
                LogLevel = NLogLoggerFactory.ToLevel(logEvent.Level),
                RenderedMessage = Layout.Render(logEvent),
                Stamp = logEvent.TimeStamp
            });
        }
    }
}
