namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AggregateSource;

    internal class WrappedFactComparerEqualityComparer : IEqualityComparer<Fact>
    {
        private readonly IFactComparer _comparer;

        public WrappedFactComparerEqualityComparer(IFactComparer comparer) => _comparer = comparer;

        bool IEqualityComparer<Fact>.Equals(Fact x, Fact y) => !_comparer.Compare(x, y).Any();

        int IEqualityComparer<Fact>.GetHashCode(Fact obj) => throw new NotSupportedException();
    }
}
