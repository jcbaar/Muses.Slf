using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Muses.Slf.Interfaces;

namespace Muses.Slf.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class UnitTests
    {
        [TestMethod]
        [TestCategory("ConcreteLoader")]
        public void ConcreteLoader_LoadFactories_ReturnsCorrectFactories()
        {
            // Arrange.
            List<ILoggerFactory> factories;

            // Ideally I would have split up these tests but since they are order dependent
            // and MSTest V2 does not support ordered testing this will have to suffice...
            // Act
            factories = ConcreteLoader.LoadFactories(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "bull");

            // Assert
            Assert.AreEqual(1, factories.Count, "Expected one logger factory to be returned.");
            Assert.IsNotNull(factories.Where(f => f.Name.Equals("NOP")), "Expected the NOP logger factory to be returned.");

            // Act
            factories = ConcreteLoader.LoadFactories();

            // Assert
            Assert.IsTrue(factories.Count >= 2, "Expected at least 2 logger factories to be returned.");
            Assert.IsNotNull(factories.Where(f => f.Name.Equals("NLog")), "The NLog logger factory was not discovered.");
            Assert.IsNotNull(factories.Where(f => f.Name.Equals("log4net")), "The log4net logger factory was not discovered.");
        }

        [TestMethod]
        [TestCategory("BaseLoggerFactory")]
        public void BaseLoggerFactory_RegisterListener_OnlyRegisteresOnce()
        {
            // Arrange
            var baseFactory = new TestFactory();
            Action<LogEvent> action = (le) => { };

            // Act and assert
            Assert.IsTrue(baseFactory.RegisterEventListener(action));
            Assert.IsFalse(baseFactory.RegisterEventListener(action));
        }

        [TestMethod]
        [TestCategory("BaseLoggerFactory")]
        public void BaseLoggerFactory_UnregisterListener_NotRegistered_Returns_False()
        {
            // Arrange
            var baseFactory = new TestFactory();
            Action<LogEvent> action = (le) => { };

            // Act and assert
            Assert.IsFalse(baseFactory.UnregisterEventListener(action));
        }

        [TestMethod]
        [TestCategory("BaseLoggerFactory")]
        public void BaseLoggerFactory_UnregisterListener_OnlyUnregisteresOnce()
        {
            // Arrange
            var baseFactory = new TestFactory();
            Action<LogEvent> action = (le) => { };

            // Act
            baseFactory.RegisterEventListener(action);

            // Assert
            Assert.IsTrue(baseFactory.UnregisterEventListener(action));
            Assert.IsFalse(baseFactory.UnregisterEventListener(action));
        }

        [TestMethod]
        [TestCategory("BaseLoggerFactory")]
        public void BaseLoggerFactory_RaiseCallsListener()
        {
            // Arrange
            var baseFactory = new TestFactory();
            bool isCalled = false, isOk = true;
            Action<LogEvent> action = (le) => { isCalled = true; };

            // Act
            isOk &= baseFactory.RegisterEventListener(action);
            isOk &= baseFactory.RaiseEvent(new LogEvent());

            // Assert
            Assert.IsTrue(isOk, "Register and/or Raise failed.");
            Assert.IsTrue(isCalled);
        }

        [TestMethod]
        [TestCategory("BaseLoggerFactory")]
        public void BaseLoggerFactory_RaiseNoListener_ReturnsFalse()
        {
            // Arrange
            var baseFactory = new TestFactory();
            bool isCalled;

            // Act
            isCalled = baseFactory.RaiseEvent(new LogEvent());

            // Assert
            Assert.IsFalse(isCalled);
        }

        [TestMethod]
        [TestCategory("Log4Net")]
        public void Log4Net_RegisterUnregisterCallback_IsOk()
        {
            Helper_RegisterUnregisterCallback_IsOk(ConcreteLoader.LoadFactories().Single(f => f.Name.Equals("log4net")));
        }

        [TestMethod]
        [TestCategory("Log4Net")]
        public void Log4Net_EventAppender_IsCalledOk()
        {
            Helper_EventCallback_IsCalledOk(ConcreteLoader.LoadFactories().Single(f => f.Name.Equals("log4net")));
        }

        [TestMethod]
        [TestCategory("NLog")]
        public void NLog_RegisterUnregisterCallback_IsOk()
        {
            Helper_RegisterUnregisterCallback_IsOk(ConcreteLoader.LoadFactories().Single(f => f.Name.Equals("NLog")));
        }

        [TestMethod]
        [TestCategory("NLog")]
        public void NLog_EventAppender_IsCalledOk()
        {
            Helper_EventCallback_IsCalledOk(ConcreteLoader.LoadFactories().Single(f => f.Name.Equals("NLog")));
        }

        public void Helper_RegisterUnregisterCallback_IsOk(ILoggerFactory factory)
        {
            // Arrange
            var action = new Action<LogEvent>((le) => { });

            // Act and assert
            Assert.IsFalse(factory.UnregisterEventListener(action));
            Assert.IsTrue(factory.RegisterEventListener(action));
            Assert.IsFalse(factory.RegisterEventListener(action));
            Assert.IsTrue(factory.UnregisterEventListener(action));

            factory.UnregisterEventListener(action);
        }

        public void Helper_EventCallback_IsCalledOk(ILoggerFactory factory)
        {
            // Arrange
            var logger = factory.GetLogger(typeof(UnitTests));
            var exception = new Exception();
            bool isTrace = false, isDebug = false, isInfo = false, isWarning = false, isError = false, isFatal = false;
            bool isTraceE = false, isDebugE = false, isInfoE = false, isWarningE = false, isErrorE = false, isFatalE = false;
            var action = new Action<LogEvent>((le) =>
            {
                bool isOk = le.RenderedMessage.Contains("Hello World!");
                if (isOk)
                {
                    if (le.Exception != null)
                    {
                        switch (le.LogLevel)
                        {
                            case Level.Trace: isTraceE = true; break;
                            case Level.Debug: isDebugE = true; break;
                            case Level.Info: isInfoE = true; break;
                            case Level.Warning: isWarningE = true; break;
                            case Level.Error: isErrorE = true; break;
                            case Level.Fatal: isFatalE = true; break;
                        }
                    }
                    else
                    {
                        switch (le.LogLevel)
                        {
                            case Level.Trace: isTrace = true; break;
                            case Level.Debug: isDebug = true; break;
                            case Level.Info: isInfo = true; break;
                            case Level.Warning: isWarning = true; break;
                            case Level.Error: isError = true; break;
                            case Level.Fatal: isFatal = true; break;
                        }
                    }
                }
            });

            factory.RegisterEventListener(action);

            // Act
            logger.Trace("Hello {0}", "World!");
            logger.Debug("Hello {0}", "World!");
            logger.Info("Hello {0}", "World!");
            logger.Warn("Hello {0}", "World!");
            logger.Error("Hello {0}", "World!");
            logger.Fatal("Hello {0}", "World!");

            logger.TraceException(exception, "Hello {0}", "World!");
            logger.DebugException(exception, "Hello {0}", "World!");
            logger.InfoException(exception, "Hello {0}", "World!");
            logger.WarnException(exception, "Hello {0}", "World!");
            logger.ErrorException(exception, "Hello {0}", "World!");
            logger.FatalException(exception, "Hello {0}", "World!");

            // Assert
            Assert.IsTrue(isTrace, "Trace failed");
            Assert.IsTrue(isDebug, "Debug failed");
            Assert.IsTrue(isInfo, "Info failed");
            Assert.IsTrue(isWarning, "Warning failed");
            Assert.IsTrue(isError, "Error failed");
            Assert.IsTrue(isFatal, "Fatal failed");

            Assert.IsTrue(isTraceE, "Trace exception failed");
            Assert.IsTrue(isDebugE, "Debug exception failed");
            Assert.IsTrue(isInfoE, "Info exception failed");
            Assert.IsTrue(isWarningE, "Warning exception failed");
            Assert.IsTrue(isErrorE, "Error exception failed");
            Assert.IsTrue(isFatalE, "Fatal exception failed");

            factory.UnregisterEventListener(action);
        }
    }
}
