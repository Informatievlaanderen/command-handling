namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System;
    using System.Linq;

    internal class TestSpecificationBuilder :
        IScenarioGivenStateBuilder,
        IScenarioGivenNoneStateBuilder,
        IScenarioWhenStateBuilder,
        IScenarioThenStateBuilder,
        IScenarioThenNoneStateBuilder,
        IScenarioThrowStateBuilder
    {
        private readonly TestSpecificationBuilderContext _context;

        public TestSpecificationBuilder() => _context = new TestSpecificationBuilderContext();

        private TestSpecificationBuilder(TestSpecificationBuilderContext context) => _context = context;

        public IScenarioGivenStateBuilder Given(params ExpectedFact[] facts)
        {
            if (facts == null)
                throw new ArgumentNullException(nameof(facts));

            return new TestSpecificationBuilder(_context.AppendGivens(facts));
        }

        public IScenarioGivenStateBuilder Given(string identifier, params object[] events)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier));

            if (events == null)
                throw new ArgumentNullException(nameof(events));

            return new TestSpecificationBuilder(_context.AppendGivens(events.Select(@event => new ExpectedFact(identifier, @event))));
        }

        public IScenarioGivenNoneStateBuilder GivenNone()
            => new TestSpecificationBuilder(_context);

        public IScenarioWhenStateBuilder When(object message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            return new TestSpecificationBuilder(_context.SetWhen(message));
        }

        public IScenarioThenStateBuilder Then(params ExpectedFact[] facts)
        {
            if (facts == null)
                throw new ArgumentNullException(nameof(facts));

            return new TestSpecificationBuilder(_context.AppendThens(facts));
        }

        public IScenarioThenStateBuilder Then(string identifier, params object[] events)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier));

            if (events == null)
                throw new ArgumentNullException(nameof(events));

            return new TestSpecificationBuilder(_context.AppendThens(events.Select(@event => new ExpectedFact(identifier, @event))));
        }

        public IScenarioThenNoneStateBuilder ThenNone()
            => new TestSpecificationBuilder(_context);

        public IScenarioThrowStateBuilder Throws(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            return new TestSpecificationBuilder(_context.SetThrows(exception));
        }

        ///// <summary>
        ///// Throws<TException>()
        ///// </summary>
        ///// <typeparam name="TException"></typeparam>
        ///// <remarks>This creates an exception of type TException by using new(), therefore the exception's message will likely not be the same.</remarks>
        //public IScenarioThrowStateBuilder Throws<TException>()
        //    where TException : Exception, new()
        //    => new TestSpecificationBuilder(_context.SetThrows<TException>());

        EventCentricTestSpecification IEventCentricTestSpecificationBuilder.Build()
            => _context.ToEventCentricSpecification();

        ExceptionCentricTestSpecification IExceptionCentricTestSpecificationBuilder.Build()
            => _context.ToExceptionCentricSpecification();
    }
}
