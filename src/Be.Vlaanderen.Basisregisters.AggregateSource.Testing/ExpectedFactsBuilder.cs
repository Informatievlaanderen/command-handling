namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System;

    /// <summary>
    /// Represents syntactic sugar to compose facts for use in the testing API.
    /// </summary>
    public class ExpectedFactsBuilder
    {
        private readonly ExpectedFact[] _facts;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpectedFactsBuilder"/> class.
        /// </summary>
        public ExpectedFactsBuilder()
            : this(ExpectedFact.Empty) { }

        private ExpectedFactsBuilder(ExpectedFact[] facts) => _facts = facts;

        /// <summary>
        /// Defines a set of events that happened to a particular aggregate.
        /// </summary>
        /// <param name="identifier">The aggregate identifier the events apply to.</param>
        /// <param name="events">The events that occurred.</param>
        /// <returns>A builder of facts.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="identifier"/> or <paramref name="events"/> is <c>null</c>.</exception>
        public ExpectedFactsBuilder That(string identifier, params object[] events)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            if (events == null)
            {
                throw new ArgumentNullException(nameof(events));
            }

            if (events.Length == 0)
            {
                return this;
            }

            var combinedFacts = new ExpectedFact[_facts.Length + events.Length];
            _facts.CopyTo(combinedFacts, 0);

            for (var index = 0; index < events.Length; index++)
            {
                combinedFacts[_facts.Length + index] = new ExpectedFact(identifier, events[index]);
            }

            return new ExpectedFactsBuilder(combinedFacts);
        }

        /// <summary>
        /// Defines a set of facts that happened.
        /// </summary>
        /// <param name="facts">The facts that occurred.</param>
        /// <returns>A builder of facts.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="facts"/> is <c>null</c>.</exception>
        public ExpectedFactsBuilder That(params ExpectedFact[] facts)
        {
            if (facts == null)
            {
                throw new ArgumentNullException(nameof(facts));
            }

            var combinedFacts = new ExpectedFact[_facts.Length + facts.Length];
            _facts.CopyTo(combinedFacts, 0);
            facts.CopyTo(combinedFacts, _facts.Length);

            return new ExpectedFactsBuilder(combinedFacts);
        }

        /// <summary>
        /// Implicitly converts a builder of facts into a testing API friendly array.
        /// </summary>
        /// <param name="builder">The builder of facts.</param>
        /// <returns>An array of facts.</returns>
        public static implicit operator ExpectedFact[] (ExpectedFactsBuilder builder) => builder._facts;
    }
}
