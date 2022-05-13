namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Tests.Comparers
{
    using System;
    using System.Collections.Generic;
    using KellermanSoftware.CompareNetObjects;
    using NUnit.Framework;
    using Testing.Comparers;

    [TestFixture]
    public class CompareNetObjectsBasedExpectedFactComparerTests
    {
        [Test]
        public void IsFactComparer()
        {
            var sut = new CompareNetObjectsBasedExpectedFactComparer(new CompareLogic());
            Assert.IsInstanceOf<IExpectedFactComparer>(sut);
        }

        [Test]
        public void CompareObjectsCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new CompareNetObjectsBasedExpectedFactComparer(null));
        }

        [Test]
        public void CompareReturnsExpectedFactWhenIdentifiersDiffer()
        {
            var comparer = new CompareLogic();
            var sut = new CompareNetObjectsBasedExpectedFactComparer(comparer);

            var @event = new Event { Value = "1" };
            var expected = new ExpectedFact("123", @event);
            var actual = new ExpectedFact("456", @event);
            var result = sut.Compare(expected, actual);

            Assert.That(result,
                Is.EquivalentTo(new[]
                {
                    new ExpectedFactComparisonDifference(expected, actual, "Expected.Identifier != Actual.Identifier (123,456)")
                }).Using(FactComparisonDifferenceComparer.Instance));
        }

        [Test]
        public void CompareReturnsExpectedFactWhenEventsDiffer()
        {
            var comparer = new CompareLogic();
            var sut = new CompareNetObjectsBasedExpectedFactComparer(comparer);

            var expected = new ExpectedFact("123", new Event { Value = "1" });
            var actual = new ExpectedFact("123", new Event { Value = "2" });
            var result = sut.Compare(expected, actual);

            Assert.That(result,
                Is.EquivalentTo(new[]
                {
                    new ExpectedFactComparisonDifference(expected, actual, "Types [String,String], Item Expected.Value != Actual.Value, Values (1,2)")
                }).Using(FactComparisonDifferenceComparer.Instance));
        }

        class Event
        {
            public string Value { get; set; }
        }

        class FactComparisonDifferenceComparer : IEqualityComparer<ExpectedFactComparisonDifference>
        {
            public static readonly IEqualityComparer<ExpectedFactComparisonDifference> Instance = new FactComparisonDifferenceComparer();

            public bool Equals(ExpectedFactComparisonDifference? x, ExpectedFactComparisonDifference? y)
            {
                return Equals(x?.Expected, y?.Expected)
                    && Equals(x?.Actual, y?.Actual)
                    && Equals(x?.Message, y?.Message);
            }

            public int GetHashCode(ExpectedFactComparisonDifference obj)
            {
                return obj.Expected.GetHashCode()
                    ^ obj.Actual.GetHashCode()
                    ^ (obj.Message != null ? obj.Message.GetHashCode() : 0);
            }
        }
    }
}
