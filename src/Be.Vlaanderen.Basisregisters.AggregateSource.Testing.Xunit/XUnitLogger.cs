namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System;
    using Xunit.Abstractions;
    using Microsoft.Extensions.Logging;

    public class XUnitLogger : ILogger
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public XUnitLogger(ITestOutputHelper testOutputHelper)
            => _testOutputHelper = testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper));

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            => _testOutputHelper.WriteLine(formatter(state, exception));

        public bool IsEnabled(LogLevel logLevel)
            => _testOutputHelper != null;

        public IDisposable BeginScope<TState>(TState state)
            => throw new NotImplementedException();
    }
}
