using log4net.Appender;
using log4net.Core;

namespace Muses.Slf.Log4Net
{
    /// <summary>
    /// A log4net <see cref="IAppender"/> which will route logging events to the registered <see cref="Muses.Slf.Interfaces.ILoggerFactory"/>
    /// callback actions.
    /// </summary>
    public class EventAppender : AppenderSkeleton
    {
        /// <summary>
        /// Called when a logging was directed to this appender.
        /// </summary>
        /// <param name="loggingEvent">The <see cref="LoggingEvent"/> describing the logging.</param>
        protected override void Append(LoggingEvent loggingEvent)
        {
            Log4NetLoggerFactory.Factory.RaiseEvent(new LogEvent
            {
                LogLevel = Log4NetLoggerFactory.ToLevel(loggingEvent.Level),
                Exception = loggingEvent.ExceptionObject,
                RenderedMessage = RenderLoggingEvent(loggingEvent),
                Stamp = loggingEvent.TimeStamp
            });
        }
    }
}
