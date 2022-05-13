namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AggregateSource;

    internal class WrappedExpectedFactComparerEqualityComparer : IEqualityComparer<ExpectedFact>
    {
        private readonly IExpectedFactComparer _comparer;

        public WrappedExpectedFactComparerEqualityComparer(IExpectedFactComparer comparer) => _comparer = comparer;

        bool IEqualityComparer<ExpectedFact>.Equals(ExpectedFact x, ExpectedFact y) => !_comparer.Compare(x, y).Any();

        int IEqualityComparer<ExpectedFact>.GetHashCode(ExpectedFact obj) => throw new NotSupportedException();
    }
}
