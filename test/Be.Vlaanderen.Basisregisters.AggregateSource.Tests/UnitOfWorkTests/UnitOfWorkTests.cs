namespace Be.Vlaanderen.Basisregisters.AggregateSource.Tests.UnitOfWorkTests
{
    using System;
    using NUnit.Framework;

    public static class Model
    {
        public static readonly string KnownIdentifier = "known/identifier";
        public static readonly string UnknownIdentifier = "unknown/identifier";
    }

    [TestFixture]
    public class WithAnyInstance
    {
        private UnitOfWork _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new UnitOfWork();
        }

        [Test]
        public void AttachNullThrows()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.Attach(null));
        }

        [Test]
        public void TryGetIdentifierCannotBeNull()
        {
            Aggregate aggregate;
            Assert.Throws<ArgumentNullException>(() => _sut.TryGet(null, out aggregate));
        }
    }

    [TestFixture]
    public class WithPristineInstance
    {
        private UnitOfWork _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new UnitOfWork();
        }

        [Test]
        public void AttachAggregateDoesNotThrow()
        {
            var aggregate = AggregateStubs.Stub1;
            Assert.DoesNotThrow(() => _sut.Attach(aggregate));
        }

        [Test]
        public void TryGetReturnsFalseAndNullAsAggregate()
        {
            var result = _sut.TryGet(Model.UnknownIdentifier, out var aggregate);

            Assert.That(result, Is.False);
            Assert.That(aggregate, Is.Null);
        }

        [Test]
        public void HasChangesReturnsFalse()
        {
            Assert.That(_sut.HasChanges(), Is.False);
        }

        [Test]
        public void GetChangesReturnsEmpty()
        {
            Assert.That(_sut.GetChanges(), Is.EquivalentTo(new Aggregate[0]));
        }
    }

    [TestFixture]
    public class WithInstanceWithAttachedAggregate
    {
        private UnitOfWork _sut;
        private Aggregate _aggregate;

        [SetUp]
        public void Setup()
        {
            _aggregate = AggregateStubs.Stub1;
            _sut = new UnitOfWork();
            _sut.Attach(_aggregate);
        }

        [Test]
        public void AttachThrowsWithSameAggregate()
        {
            Assert.Throws<ArgumentException>(() => _sut.Attach(_aggregate));
        }

        [Test]
        public void AttachDoesNotThrowWithOtherAggregate()
        {
            var otherAggregate = AggregateStubs.Stub2;
            Assert.DoesNotThrow(() => _sut.Attach(otherAggregate));
        }

        [Test]
        public void TryGetReturnsFalseAndNullAsAggregateForUnknownId()
        {
            var result = _sut.TryGet(Model.UnknownIdentifier, out var aggregate);

            Assert.That(result, Is.False);
            Assert.That(aggregate, Is.Null);
        }

        [Test]
        public void TryGetReturnsTrueAndAggregateForKnownId()
        {
            var result = _sut.TryGet(_aggregate.Identifier, out var aggregate);

            Assert.That(result, Is.True);
            Assert.That(aggregate, Is.SameAs(_aggregate));
        }

        [Test]
        public void HasChangesReturnsFalse()
        {
            Assert.That(_sut.HasChanges(), Is.False);
        }

        [Test]
        public void GetChangesReturnsEmpty()
        {
            Assert.That(_sut.GetChanges(), Is.EquivalentTo(new Aggregate[0]));
        }
    }

    [TestFixture]
    public class WithInstanceWithAttachedChangedAggregates
    {
        private UnitOfWork _sut;
        private Aggregate _aggregate1;
        private Aggregate _aggregate2;

        [SetUp]
        public void Setup()
        {
            _aggregate1 = AggregateStubs.Create(new ChangedAggregateRootEntityStub());
            _aggregate2 = AggregateStubs.Create(new ChangedAggregateRootEntityStub());
            _sut = new UnitOfWork();
            _sut.Attach(_aggregate1);
            _sut.Attach(_aggregate2);
        }

        [Test]
        public void HasChangesReturnsTrue()
        {
            Assert.That(_sut.HasChanges(), Is.True);
        }

        [Test]
        public void GetChangesReturnsEmpty()
        {
            Assert.That(_sut.GetChanges(), Is.EquivalentTo(new[] { _aggregate1, _aggregate2 }));
        }
    }

    internal class ChangedAggregateRootEntityStub : AggregateRootEntity
    {
        public ChangedAggregateRootEntityStub()
        {
            ApplyChange(new object());
        }
    }
}
