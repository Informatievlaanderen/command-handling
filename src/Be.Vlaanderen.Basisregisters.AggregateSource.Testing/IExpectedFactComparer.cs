namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System.Collections.Generic;

    /// <summary>
    /// Contract to compare if the expected and actual fact are equal.
    /// </summary>
    public interface IExpectedFactComparer
    {
        /// <summary>
        /// Compares the expected to the actual fact.
        /// </summary>
        /// <param name="expected">The expected fact.</param>
        /// <param name="actual">The actual fact.</param>
        /// <returns>An enumeration of <see cref="ExpectedFactComparisonDifference">differences</see>, or empty if none found.</returns>
        IEnumerable<ExpectedFactComparisonDifference> Compare(ExpectedFact expected, ExpectedFact actual);
    }
}
