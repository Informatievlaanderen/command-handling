namespace Be.Vlaanderen.Basisregisters.AggregateSource.Tests
{
    using System;
    using System.Globalization;

    internal class TestExpectedFactBuilder
    {
        private readonly string _identifier;
        private readonly object _event;

        public TestExpectedFactBuilder()
        {
            _identifier = new Random().Next().ToString(CultureInfo.CurrentCulture);
            _event = new object();
        }

        private TestExpectedFactBuilder(string identifier, object @event)
        {
            _identifier = identifier;
            _event = @event;
        }

        public TestExpectedFactBuilder WithEvent(object @event) => new TestExpectedFactBuilder(_identifier, @event);

        public TestExpectedFactBuilder WithIdentifier(string identifier) => new TestExpectedFactBuilder(identifier, _event);

        public ExpectedFact Build() => new ExpectedFact(_identifier, _event);
    }
}
