namespace Be.Vlaanderen.Basisregisters.AggregateSource.Tests.ValueObjectTests
{
    using System.Collections.Generic;
    using NodaTime;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public class ValueObjectTests
    {
        private class Address : ValueObject<Address>
        {
            private string Address1 { get; set; }

            private string City { get; set; }

            private string State { get; set; }

            public Address(string address1, string city, string state)
            {
                Address1 = address1;
                City = city;
                State = state;
            }

            protected override IEnumerable<object> Reflect()
            {
                yield return Address1;
                yield return City;
                yield return State;
            }
        }

        private class ExpandedAddress : Address
        {
            private string Address2 { get; }

            public ExpandedAddress(string address1, string address2, string city, string state)
                : base(address1, city, state)
            {
                Address2 = address2;
            }

            protected override IEnumerable<object> Reflect()
            {
                yield return base.Reflect();
                yield return Address2;
            }
        }

        [Test]
        public void AddressEqualsWorksWithIdenticalAddresses()
        {
            var address = new Address("Address1", "Austin", "TX");
            var address2 = new Address("Address1", "Austin", "TX");

            ClassicAssert.IsTrue(address.Equals(address2));
        }

        [Test]
        public void AddressEqualsWorksWithNonIdenticalAddresses()
        {
            var address = new Address("Address1", "Austin", "TX");
            var address2 = new Address("Address2", "Austin", "TX");

            ClassicAssert.IsFalse(address.Equals(address2));
        }

        [Test]
        public void AddressEqualsWorksWithNulls()
        {
            var address = new Address(null, "Austin", "TX");
            var address2 = new Address("Address2", "Austin", "TX");

            ClassicAssert.IsFalse(address.Equals(address2));
        }

        [Test]
        public void AddressEqualsWorksWithNullsOnOtherObject()
        {
            var address = new Address("Address2", "Austin", "TX");
            var address2 = new Address("Address2", null, "TX");

            ClassicAssert.IsFalse(address.Equals(address2));
        }

        [Test]
        public void AddressEqualsIsReflexive()
        {
            var address = new Address("Address1", "Austin", "TX");

            ClassicAssert.IsTrue(address.Equals(address));
        }

        [Test]
        public void AddressEqualsIsSymmetric()
        {
            var address = new Address("Address1", "Austin", "TX");
            var address2 = new Address("Address2", "Austin", "TX");

            ClassicAssert.IsFalse(address.Equals(address2));
            ClassicAssert.IsFalse(address2.Equals(address));
        }

        [Test]
        public void AddressEqualsIsTransitive()
        {
            var address = new Address("Address1", "Austin", "TX");
            var address2 = new Address("Address1", "Austin", "TX");
            var address3 = new Address("Address1", "Austin", "TX");

            ClassicAssert.IsTrue(address.Equals(address2));
            ClassicAssert.IsTrue(address2.Equals(address3));
            ClassicAssert.IsTrue(address.Equals(address3));
        }

        [Test]
        public void AddressOperatorsWork()
        {
            var address = new Address("Address1", "Austin", "TX");
            var address2 = new Address("Address1", "Austin", "TX");
            var address3 = new Address("Address2", "Austin", "TX");

            ClassicAssert.IsTrue(address == address2);
            ClassicAssert.IsTrue(address2 != address3);
        }

        [Test]
        public void DerivedTypesBehaveCorrectly()
        {
            var address = new Address("Address1", "Austin", "TX");
            var address2 = new ExpandedAddress("Address1", "Apt 123", "Austin", "TX");

            ClassicAssert.IsFalse(address.Equals(address2));
            ClassicAssert.IsFalse(address == address2);
        }

        [Test]
        public void EqualValueObjectsHaveSameHashCode()
        {
            var address = new Address("Address1", "Austin", "TX");
            var address2 = new Address("Address1", "Austin", "TX");

            ClassicAssert.AreEqual(address.GetHashCode(), address2.GetHashCode());
        }

        [Test]
        public void TransposedValuesGiveDifferentHashCodes()
        {
            var address = new Address(null, "Austin", "TX");
            var address2 = new Address("TX", "Austin", null);

            ClassicAssert.AreNotEqual(address.GetHashCode(), address2.GetHashCode());
        }

        [Test]
        public void UnequalValueObjectsHaveDifferentHashCodes()
        {
            var address = new Address("Address1", "Austin", "TX");
            var address2 = new Address("Address2", "Austin", "TX");

            ClassicAssert.AreNotEqual(address.GetHashCode(), address2.GetHashCode());
        }

        [Test]
        public void TransposedValuesOfFieldNamesGivesDifferentHashCodes()
        {
            var address = new Address("_city", null, null);
            var address2 = new Address(null, "_address1", null);

            ClassicAssert.AreNotEqual(address.GetHashCode(), address2.GetHashCode());
        }

        [Test]
        public void DerivedTypesHashCodesBehaveCorrectly()
        {
            var address = new ExpandedAddress("Address99999", "Apt 123", "New Orleans", "LA");
            var address2 = new ExpandedAddress("Address1", "Apt 123", "Austin", "TX");

            ClassicAssert.AreNotEqual(address.GetHashCode(), address2.GetHashCode());
        }

        [Test]
        public void OffsetDateTimeTests()
        {
            var time = new MyOffsetTime(new OffsetDateTime(new LocalDateTime(2000, 1, 10, 0, 0, 0), Offset.FromHours(1)));
            var timeAfter = new MyOffsetTime(new OffsetDateTime(new LocalDateTime(2000, 1, 10, 0, 0, 1), Offset.FromHours(1)));
            var timeBefore = new MyOffsetTime(new OffsetDateTime(new LocalDateTime(2000, 1, 9, 0, 0, 59), Offset.FromHours(1)));

            ClassicAssert.Greater(timeAfter, time);
            ClassicAssert.Less(timeBefore, time);
        }

        [Test]
        public void ZonedDateTimeTests()
        {
            var time = new MyZonedTime(new ZonedDateTime(Instant.FromUtc(2000, 1, 10, 0, 0, 0), DateTimeZone.Utc));
            var timeAfter = new MyZonedTime(new ZonedDateTime(Instant.FromUtc(2000, 1, 10, 0, 0, 1), DateTimeZone.Utc));
            var timeBefore = new MyZonedTime(new ZonedDateTime(Instant.FromUtc(2000, 1, 9, 0, 0, 59), DateTimeZone.Utc));

            ClassicAssert.Greater(timeAfter, time);
            ClassicAssert.Less(timeBefore, time);
        }

        [Test]
        public void LocalDateTimeTests()
        {
            var time = new MyLocalTime(new LocalDateTime(2000, 1, 10, 0, 0, 0, 0));
            var timeAfter = new MyLocalTime(new LocalDateTime(2000, 1, 10, 0, 0, 1, 0));
            var timeBefore = new MyLocalTime(new LocalDateTime(2000, 1, 9, 0, 0, 59, 0));

            ClassicAssert.Greater(timeAfter, time);
            ClassicAssert.Less(timeBefore, time);
        }
    }

    public class MyOffsetTime : OffsetDateTimeValueObject<MyOffsetTime>
    {
        public MyOffsetTime(OffsetDateTime offsetDateTime) : base(offsetDateTime) { }
    }

    public class MyZonedTime : ZonedDateTimeValueObject<MyZonedTime>
    {
        public MyZonedTime(ZonedDateTime zonedDateTime) : base(zonedDateTime) { }
    }

    public class MyLocalTime : LocalDateTimeValueObject<MyLocalTime>
    {
        public MyLocalTime(LocalDateTime localDateTime) : base(localDateTime) { }
    }
}
