namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    /// <summary>
    /// Represents a syntactic sugar bootstrapper to compose facts for use in the testing API.
    /// </summary>
    public static class State
    {
        /// <summary>
        /// Defines a set of events that happened to a particular aggregate.
        /// </summary>
        /// <param name="identifier">The aggregate identifier the events apply to.</param>
        /// <param name="events">The events that occurred.</param>
        /// <returns>A builder of facts.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="identifier"/> or <paramref name="events"/> is <c>null</c>.</exception>
        public static ExpectedFactsBuilder That(string identifier, params object[] events) => new ExpectedFactsBuilder().That(identifier, events);

        /// <summary>
        /// Defines a set of facts that happened.
        /// </summary>
        /// <param name="facts">The facts that occurred.</param>
        /// <returns>A builder of facts.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="facts"/> is <c>null</c>.</exception>
        public static ExpectedFactsBuilder That(params ExpectedFact[] facts) => new ExpectedFactsBuilder().That(facts);
    }
}
