namespace Be.Vlaanderen.Basisregisters.AggregateSource.Tests
{
    using System;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public class FactTests
    {
        private TestFactBuilder _sutBuilder;

        [SetUp]
        public void SetUp()
        {
            _sutBuilder = new TestFactBuilder();
        }

        [Test]
        public void IdentifierCanNotBeNull()
        {
            ClassicAssert.Throws<ArgumentNullException>(() => _sutBuilder.WithIdentifier(null).Build());
        }

        [Test]
        public void EventCanNotBeNull()
        {
            ClassicAssert.Throws<ArgumentNullException>(() => _sutBuilder.WithEvent(null).Build());
        }

        [Test]
        public void TwoInstancesAreEqualWhenTheirIdentifierAndEventAreEqual()
        {
            ClassicAssert.AreEqual(_sutBuilder.Build(), _sutBuilder.Build());
        }

        [Test]
        public void TwoInstancesAreNotEqualWhenTheirIdentifierDiffers()
        {
            ClassicAssert.AreNotEqual(
                _sutBuilder.WithIdentifier("123").Build(),
                _sutBuilder.WithIdentifier("456").Build());
        }

        [Test]
        public void TwoInstancesAreNotEqualWhenTheirEventDiffers()
        {
            ClassicAssert.AreNotEqual(
                _sutBuilder.WithEvent(new object()).Build(),
                _sutBuilder.WithEvent(new object()).Build());
        }

        [Test]
        public void TwoInstancesHaveTheSameHashCodeWhenTheirIdentifierAndEventAreEqual()
        {
            ClassicAssert.AreEqual(_sutBuilder.Build().GetHashCode(), _sutBuilder.Build().GetHashCode());
        }

        [Test]
        public void TwoInstancesDoNotHaveTheSameHashCodeWhenTheirIdentifierDiffers()
        {
            ClassicAssert.AreNotEqual(
                _sutBuilder.WithIdentifier("123").Build().GetHashCode(),
                _sutBuilder.WithIdentifier("456").Build().GetHashCode());
        }

        [Test]
        public void TwoInstancesDoNotHaveTheSameHashCodeWhenTheirEventDiffers()
        {
            ClassicAssert.AreNotEqual(
                _sutBuilder.WithEvent(new object()).Build().GetHashCode(),
                _sutBuilder.WithEvent(new object()).Build().GetHashCode());
        }

        [Test]
        public void IsEquatable()
        {
            ClassicAssert.That(_sutBuilder.Build(), Is.InstanceOf<IEquatable<Fact>>());
        }

        [Test]
        public void DoesObjectEqualItself()
        {
            var instance = _sutBuilder.Build();
            ClassicAssert.IsTrue(instance.Equals((object)instance));
        }

        [Test]
        public void DoesNotEqualObjectOfOtherType()
        {
            ClassicAssert.IsFalse(_sutBuilder.Build().Equals(new object()));
        }

        [Test]
        public void DoesNotEqualNullAsObject()
        {
            ClassicAssert.IsFalse(_sutBuilder.Build().Equals(null));
        }

        [Test]
        public void UsingConstructorReturnsInstanceWithExpectedProperties()
        {
            var @event = new object();
            var sut = _sutBuilder.WithIdentifier("123").WithEvent(@event).Build();

            ClassicAssert.That(sut.Identifier, Is.EqualTo("123"));
            ClassicAssert.That(sut.Event, Is.SameAs(@event));
        }

        [Test]
        public void ImplicitConversionToTupleReturnsExpectedResult()
        {
            var @event = new object();
            var sut = _sutBuilder.WithIdentifier("123").WithEvent(@event).Build();

            Tuple<string, object> result = sut;

            ClassicAssert.That(result.Item1, Is.EqualTo("123"));
            ClassicAssert.That(result.Item2, Is.SameAs(@event));
        }
    }
}
