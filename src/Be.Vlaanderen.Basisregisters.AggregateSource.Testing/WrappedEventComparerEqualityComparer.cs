namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class WrappedEventComparerEqualityComparer : IEqualityComparer<object>
    {
        private readonly IEventComparer _comparer;

        public WrappedEventComparerEqualityComparer(IEventComparer comparer) => _comparer = comparer;

        bool IEqualityComparer<object>.Equals(object x, object y) => !_comparer.Compare(x, y).Any();

        int IEqualityComparer<object>.GetHashCode(object obj) => throw new NotSupportedException();
    }
}
