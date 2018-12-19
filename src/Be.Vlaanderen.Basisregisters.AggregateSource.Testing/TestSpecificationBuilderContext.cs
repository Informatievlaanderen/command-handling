namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class TestSpecificationBuilderContext
    {
        private readonly Fact[] _givens;
        private readonly Fact[] _thens;
        private readonly object _when;
        private readonly Exception _throws;

        public TestSpecificationBuilderContext()
        {
            _givens = Fact.Empty;
            _thens = Fact.Empty;
            _when = null;
            _throws = null;
        }

        private TestSpecificationBuilderContext(
            Fact[] givens,
            object when,
            Fact[] thens,
            Exception throws)
        {
            _givens = givens;
            _when = when;
            _thens = thens;
            _throws = throws;
        }

        public TestSpecificationBuilderContext AppendGivens(IEnumerable<Fact> facts)
            => new TestSpecificationBuilderContext(_givens.Concat(facts).ToArray(), _when, _thens, _throws);

        public TestSpecificationBuilderContext SetWhen(object message)
            => new TestSpecificationBuilderContext(_givens, message, _thens, _throws);

        public TestSpecificationBuilderContext AppendThens(IEnumerable<Fact> facts)
            => new TestSpecificationBuilderContext(_givens, _when, _thens.Concat(facts).ToArray(), _throws);

        public TestSpecificationBuilderContext SetThrows(Exception exception)
            => new TestSpecificationBuilderContext(_givens, _when, _thens, exception);

        public EventCentricTestSpecification ToEventCentricSpecification()
            => new EventCentricTestSpecification(_givens, _when, _thens);

        public ExceptionCentricTestSpecification ToExceptionCentricSpecification()
            => new ExceptionCentricTestSpecification(_givens, _when, _throws);
    }
}
