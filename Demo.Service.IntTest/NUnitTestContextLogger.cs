using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;

namespace Demo.Service.IntTest
{
    public static class NUnitTestContextLoggerExtensions
    {
        public static ILoggingBuilder AddNUnit(this ILoggingBuilder builder)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, NUnitTestContextLoggerProvider>());
            return builder;
        }
    }

    public class NUnitTestContextLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string name) => new NUnitTestContextLogger(name);

        public void Dispose() { }
    }

    // Copied on https://github.com/dotnet/runtime/blob/master/src/libraries/Microsoft.Extensions.Logging.Debug/src/DebugLogger.cs
    public class NUnitTestContextLogger : ILogger
    {
        public NUnitTestContextLogger(string name)
        {
            _name = name;
        }

        private readonly string _name;

        public IDisposable BeginScope<TState>(TState state)
        {
            return NullScope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            string message = formatter(state, exception);

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            message = $"{logLevel}: {_name}: {message}";

            if (exception != null)
            {
                message += Environment.NewLine + exception;
            }

            TestContext.WriteLine(message);
        }
    }

    internal class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new NullScope();

        private NullScope() { }

        public void Dispose() { }
    }
}
