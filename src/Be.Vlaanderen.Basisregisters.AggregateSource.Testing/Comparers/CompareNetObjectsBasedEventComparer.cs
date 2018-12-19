namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Comparers
{
    using System;
    using System.Collections.Generic;
    using KellermanSoftware.CompareNetObjects;

    /// <summary>
    /// Compares events using a <see cref="T:KellermanSoftware.CompareNetObjects.ICompareLogic" /> object and reports the differences.
    /// </summary>
    public class CompareNetObjectsBasedEventComparer : IEventComparer
    {
        private readonly ICompareLogic _logic;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompareNetObjectsBasedEventComparer"/> class.
        /// </summary>
        /// <param name="logic">The compare logic.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="logic"/> is <c>null</c>.</exception>
        public CompareNetObjectsBasedEventComparer(ICompareLogic logic) => _logic = logic ?? throw new ArgumentNullException(nameof(logic));

        /// <summary>
        /// Compares the expected to the actual event.
        /// </summary>
        /// <param name="expected">The expected event.</param>
        /// <param name="actual">The actual event.</param>
        /// <returns>
        /// An enumeration of <see cref="T:Be.Vlaanderen.Basisregisters.AggregateSource.Testing.EventComparisonDifference">differences</see>, or empty if none found.
        /// </returns>
        public IEnumerable<EventComparisonDifference> Compare(object expected, object actual)
        {
            var result = _logic.Compare(expected, actual);
            if (result.AreEqual)
                yield break;

            foreach (var difference in result.Differences)
            {
                yield return new EventComparisonDifference(
                    expected, 
                    actual,
                    difference.ToString());
            }
        }
    }
}
