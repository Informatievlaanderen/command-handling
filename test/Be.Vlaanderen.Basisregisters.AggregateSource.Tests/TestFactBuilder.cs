namespace Be.Vlaanderen.Basisregisters.AggregateSource.Tests
{
    using System;
    using System.Globalization;

    internal class TestFactBuilder
    {
        private readonly string _identifier;
        private readonly object _event;

        public TestFactBuilder()
        {
            _identifier = new Random().Next().ToString(CultureInfo.CurrentCulture);
            _event = new object();
        }

        private TestFactBuilder(string identifier, object @event)
        {
            _identifier = identifier;
            _event = @event;
        }

        public TestFactBuilder WithEvent(object @event) => new TestFactBuilder(_identifier, @event);

        public TestFactBuilder WithIdentifier(string identifier) => new TestFactBuilder(identifier, _event);

        public Fact Build() => new Fact(_identifier, _event);
    }
}
