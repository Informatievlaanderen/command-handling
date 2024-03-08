namespace Be.Vlaanderen.Basisregisters.AggregateSource.Tests.EventRecorderTests
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public class WithAnyInstance
    {
        private EventRecorder _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new EventRecorder();
        }

        [Test]
        public void IsEnumerable()
        {
            ClassicAssert.IsInstanceOf<IEnumerable<object>>(_sut);
        }

        [Test]
        public void RecordEventCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.Record(null));
        }
    }

    [TestFixture]
    public class WithEmptyInstance
    {
        private EventRecorder _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new EventRecorder();
        }

        [Test]
        public void ResetDoesNotThrow()
        {
            Assert.DoesNotThrow(() => _sut.Reset());
        }

        [Test]
        public void IsStillEmptyAfterReset()
        {
            _sut.Reset();
            ClassicAssert.IsEmpty(_sut);
        }

        [Test]
        public void IsEmpty()
        {
            ClassicAssert.IsEmpty(_sut);
        }

        [Test]
        public void RecordDoesNotThrow()
        {
            Assert.DoesNotThrow(() => _sut.Record(new object()));
        }

        [Test]
        public void ContainsExpectedEventsAfterRecord()
        {
            var initialEvent = new object();
            _sut.Record(initialEvent);
            Assert.That(_sut, Is.EquivalentTo(new[] { initialEvent }));
        }
    }

    [TestFixture]
    public class WithMutatedInstance
    {
        private EventRecorder _sut;
        private object _initialEvent;

        [SetUp]
        public void Setup()
        {
            _sut = new EventRecorder();
            _initialEvent = new object();
            _sut.Record(_initialEvent);
        }

        [Test]
        public void ResetDoesNotThrow()
        {
            Assert.DoesNotThrow(() => _sut.Reset());
        }

        [Test]
        public void IsEmptyAfterReset()
        {
            _sut.Reset();
            ClassicAssert.IsEmpty(_sut);
        }

        [Test]
        public void IsNotEmpty()
        {
            ClassicAssert.IsNotEmpty(_sut);
        }

        [Test]
        public void ContainsExpectedEvents()
        {
            Assert.That(_sut, Is.EquivalentTo(new[] { _initialEvent }));
        }

        [Test]
        public void RecordDoesNotThrow()
        {
            Assert.DoesNotThrow(() => _sut.Record(new object()));
        }

        [Test]
        public void ContainsExpectedEventsAfterRecord()
        {
            var nextEvent = new object();
            _sut.Record(nextEvent);
            Assert.That(_sut, Is.EquivalentTo(new[] { _initialEvent, nextEvent }));
        }
    }
}
