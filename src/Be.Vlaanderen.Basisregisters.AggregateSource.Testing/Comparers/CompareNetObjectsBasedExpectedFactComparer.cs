namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Comparers
{
    using System;
    using System.Collections.Generic;
    using KellermanSoftware.CompareNetObjects;

    /// <summary>
    /// Compares facts using a <see cref="T:KellermanSoftware.CompareNetObjects.ICompareLogic" /> object and reports the differences.
    /// </summary>
    public class CompareNetObjectsBasedExpectedFactComparer : IExpectedFactComparer
    {
        private readonly ICompareLogic _logic;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompareNetObjectsBasedFactComparer"/> class.
        /// </summary>
        /// <param name="logic">The comparer.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="logic">comparer</paramref> is <c>null</c>.</exception>
        public CompareNetObjectsBasedExpectedFactComparer(ICompareLogic logic) => _logic = logic ?? throw new ArgumentNullException(nameof(logic));

        /// <summary>
        /// Compares the expected to the actual fact.
        /// </summary>
        /// <param name="expected">The expected fact.</param>
        /// <param name="actual">The actual fact.</param>
        /// <returns>
        /// An enumeration of <see cref="ExpectedFactComparisonDifference">differences</see>, or empty if none found.
        /// </returns>
        public IEnumerable<ExpectedFactComparisonDifference> Compare(ExpectedFact expected, ExpectedFact actual)
        {
            if (string.CompareOrdinal(expected.Identifier, actual.Identifier) != 0)
            {
                yield return new ExpectedFactComparisonDifference(
                    expected,
                    actual,
                    $"Expected.Identifier != Actual.Identifier ({expected.Identifier},{actual.Identifier})");
            }

            var result = _logic.Compare(expected.Event, actual.Event);
            if (result.AreEqual)
            {
                yield break;
            }

            foreach (var difference in result.Differences)
            {
                yield return new ExpectedFactComparisonDifference(
                    expected,
                    actual,
                    difference.ToString());
            }
        }
    }
}
