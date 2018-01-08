using Muses.Slf.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Muses.Slf
{
    /// <summary>
    /// Static helper class which tries to load and instantiate a concrete copy of
    /// all classes that implement the <see cref="ILoggerFactory"/> interface.
    /// </summary>
    public static class ConcreteLoader
    {
        private static object _lock = new object();
        private static List<ILoggerFactory> _factories = null;

        /// <summary>
        /// Loads and instantiates the <see cref="ILoggerFactory"/> implementing classes from the
        /// DLL assemblies found in the application startup folder. The default naming pattern looked for
        /// in the assemblies is 'Muses.Slf.*.dll'
        /// </summary>
        /// <returns>A <see cref="List{ILoggerFactory}"/> containing the concrete, <see cref="ILoggerFactory"/> implementing
        /// objects. When no classes implementing the <see cref="ILoggerFactory"/> class where found a list with a single
        /// <see cref="NopLoggerFactory"/> instance is returned. This will not actually log anything but it will make the
        /// code run without any other issues.</returns>
        public static List<ILoggerFactory> LoadFactories()
        {
            return LoadFactories(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Muses.Slf.*.dll");
        }

        /// <summary>
        /// Loads and instantiates the <see cref="ILoggerFactory"/> implementing classes from the
        /// assemblies matching the <paramref name="pattern"/> wild-cards found in the <paramref name="directory"/> folder.
        /// </summary>
        /// <returns>A <see cref="List{ILoggerFactory}"/> containing the concrete, <see cref="ILoggerFactory"/> implementing
        /// objects. When no classes implementing the <see cref="ILoggerFactory"/> class where found a list with a single
        /// <see cref="NopLoggerFactory"/> instance is returned. This will not actually log anything but it will make the
        /// code run without any other issues.</returns>
        public static List<ILoggerFactory> LoadFactories(string directory, string pattern)
        {
            lock (_lock)
            {
                // We only load the list once.
                if (_factories == null)
                {
                    _factories = new List<ILoggerFactory>();
                    try
                    {
                        var assemblies = Directory.GetFiles(directory, pattern);
                        foreach (var assembly in assemblies)
                        {
                            var a = Assembly.LoadFile(assembly);

                            // Select all types that:
                            // 1) Implement the ILoggerFactory interface.
                            // 2) Are not the interface itself.
                            // 3) Are not the NopLogger type.
                            var all = a.GetTypes();
                            var types = all.Where(t => typeof(ILoggerFactory).IsAssignableFrom(t) && !t.IsInterface && !t.Equals(typeof(NopLoggerFactory))).Select(t => t);
                            if (types.Any())
                            {
                                foreach (var type in types)
                                {
                                    _factories.Add((ILoggerFactory)Activator.CreateInstance(type, true));
                                }
                            }
                        }
                    }
                    finally
                    {
                        if (_factories.Count == 0)
                        {
                            // When no concrete types are found we need to add the
                            // NopLoggerFactory so that we at least have a logging factory. This
                            // NopLoggerFactory will serve up a NopLogger instance which simply
                            // does nothing...
                            _factories.Add(new NopLoggerFactory());
                        }
                    }
                }
            }
            return _factories;
        }
    }
}
