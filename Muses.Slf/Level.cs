namespace Muses.Slf
{
    /// <summary>
    /// Simple logging levels.
    /// </summary>
    public enum Level
    {
        /// <summary>
        /// Log level was of an unknown type.
        /// </summary>
        Other,

        /// <summary>
        /// Tracing log level.
        /// </summary>
        Trace,

        /// <summary>
        /// Debug log level.
        /// </summary>
        Debug,

        /// <summary>
        /// Informational log level.
        /// </summary>
        Info,

        /// <summary>
        /// Warning log level.
        /// </summary>
        Warning,

        /// <summary>
        /// Error log level.
        /// </summary>
        Error,

        /// <summary>
        /// Fatal log level.
        /// </summary>
        Fatal
    }
}
