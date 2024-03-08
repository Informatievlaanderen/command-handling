namespace Be.Vlaanderen.Basisregisters.AggregateSource.Tests.OptionalTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public class WithAnyInstance
    {
        private Optional<AggregateRootEntityStub> _sut;

        [SetUp]
        public void Setup()
        {
            _sut = Optional<AggregateRootEntityStub>.Empty;
        }

        [Test]
        public void IsEquatable()
        {
            ClassicAssert.That(_sut,
                        Is.InstanceOf<IEquatable<Optional<AggregateRootEntityStub>>>());
        }
    }

    [TestFixture]
    public class WithEmptyReferenceTypeInstance : WithEmptyInstanceFixture<Object>
    {
        protected override Optional<object> SutFactory()
        {
            return Optional<object>.Empty;
        }

        protected override Optional<object> WithValueFactory()
        {
            return new Optional<object>(new object());
        }
    }

    [TestFixture]
    public class WithEmptyValueTypeInstance : WithEmptyInstanceFixture<Int32>
    {
        protected override Optional<int> SutFactory()
        {
            return Optional<int>.Empty;
        }

        protected override Optional<int> WithValueFactory()
        {
            return new Optional<int>(123);
        }
    }

    [TestFixture]
    public abstract class WithEmptyInstanceFixture<T>
    {
        private Optional<T> _sut;

        [SetUp]
        public void Setup()
        {
            _sut = SutFactory();
        }

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sut")]
        protected abstract Optional<T> SutFactory();
        protected abstract Optional<T> WithValueFactory();

        [Test]
        public void HasValueReturnsFalse()
        {
            ClassicAssert.That(_sut.HasValue, Is.False);
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "_")]
        [Test]
        public void ValueThrows()
        {
            // ReSharper disable UnusedVariable
            ClassicAssert.Throws<InvalidOperationException>(() => { var _ = _sut.Value; });
            // ReSharper restore UnusedVariable
        }

        [Test]
        public void TypedEnumerationIsEmpty()
        {
            ClassicAssert.That(_sut, Is.EquivalentTo(new T[0]));
        }

        [Test]
        public void ObjectEnumerationIsEmpty()
        {
            var sut = (IEnumerable) _sut;

            ClassicAssert.That(sut, Is.EquivalentTo(new T[0]));
        }

        [Test]
        public void DoesNotEqualNullUsingEquals()
        {
            ClassicAssert.IsFalse(_sut.Equals(null));
        }

        [Test]
        public void DoesNotEqualNullUsingEqualityOperator()
        {
            ClassicAssert.IsFalse(_sut == null);
        }

        [Test]
        public void DiffersFromNullUsingInequalityOperator()
        {
            ClassicAssert.IsTrue(_sut != null);
        }

        [Test]
        public void DoesNotObjectEqualNull()
        {
            // ReSharper disable RedundantCast
            ClassicAssert.IsFalse(_sut.Equals((object) null));
            // ReSharper restore RedundantCast
        }

        [Test]
        public void DoesNotEqualObjectOfOtherType()
        {
            ClassicAssert.IsFalse(_sut.Equals(new object()));
        }

        [Test]
        public void DoesObjectEqualItself()
        {
            ClassicAssert.IsTrue(_sut.Equals((object) _sut));
        }

        [Test]
        public void DoesEqualItself()
        {
            ClassicAssert.IsTrue(_sut.Equals(_sut));
        }

        [Test]
        public void DoesEqualItselfUsingEqualityOperator()
        {
            var @ref = _sut;
            ClassicAssert.IsTrue(_sut == @ref);
        }

        [Test]
        public void DoesNotDifferFromItselfUsingInequalityOperator()
        {
            var @ref = _sut;
            ClassicAssert.IsFalse(_sut != @ref);
        }

        [Test]
        public void TwoInstancesAreEqualIfTheyBothDoNotHaveAValueAndAreOfTheSameValueType()
        {
            ClassicAssert.IsTrue(_sut.Equals(Optional<T>.Empty));
        }

        [Test]
        public void TwoInstancesAreEqualUsingTheEqualityOperatorIfTheyBothDoNotHaveAValue()
        {
            ClassicAssert.IsTrue(_sut == Optional<T>.Empty);
        }

        [Test]
        public void TwoInstancesDoNotDifferUsingTheInequalityOperatorIfTheyBothDoNotHaveAValue()
        {
            ClassicAssert.IsFalse(_sut != Optional<T>.Empty);
        }

        [Test]
        public void TwoInstancesAreNotEqualIfTheOtherHasAValueAndIsOfTheSameValueType()
        {
            ClassicAssert.IsFalse(_sut.Equals(WithValueFactory()));
        }

        [Test]
        public void TwoInstancesAreNotEqualUsingTheEqualityOperatorIfTheOtherHasAValue()
        {
            ClassicAssert.IsFalse(_sut == WithValueFactory());
        }

        [Test]
        public void TwoInstancesDifferUsingTheEqualityOperatorIfTheOtherHasAValue()
        {
            ClassicAssert.IsTrue(_sut != WithValueFactory());
        }

        [Test]
        public void TwoInstancesAreNotEqualIfTheOtherHasAValueAndIsOfDifferentValueType()
        {
            ClassicAssert.IsFalse(_sut.Equals(new Optional<MismatchObject>(new MismatchObject())));
        }

        [Test]
        public void TwoInstancesAreNotEqualIfTheOtherHasNoValueAndIsOfDifferentValueType()
        {
            ClassicAssert.IsFalse(_sut.Equals(Optional<MismatchObject>.Empty));
        }

        [Test]
        public void TwoInstanceHaveTheSameHashCodeIfTheyBothDoNotHaveAValueAndAreOfTheSameValueType()
        {
            ClassicAssert.IsTrue(_sut.GetHashCode().Equals(Optional<T>.Empty.GetHashCode()));
        }

        [Test]
        public void TwoInstancesDoNotHaveTheSameHashCodeIfTheOtherHasAValueAndIsOfTheSameValueType()
        {
            ClassicAssert.IsFalse(_sut.GetHashCode().Equals(WithValueFactory().GetHashCode()));
        }

        [Test]
        public void TwoInstancesDoNotHaveTheSameHashCodeIfTheOtherHasAValueAndIsOfDifferentValueType()
        {
            ClassicAssert.IsFalse(_sut.GetHashCode().
                                Equals(new Optional<MismatchObject>(new MismatchObject()).GetHashCode()));
        }

        [Test]
        public void TwoInstancesDoNotHaveTheSameHashCodeIfTheOtherHasNoValueAndIsOfDifferentValueType()
        {
            ClassicAssert.IsFalse(_sut.GetHashCode().
                                Equals(Optional<MismatchObject>.Empty.GetHashCode()));
        }
    }

    [TestFixture]
    public class WithFilledObjectArray : WithFilledInstance<object[]>
    {
        protected override object[] ValueFactory()
        {
            return new[] { new object(), new object() };
        }

        protected override Optional<object[]> InstanceFactory(object[] value)
        {
            return new Optional<object[]>(value);
        }

        [Test]
        public void TwoInstancesAreEqualIfTheirValuesAreDifferentArrayInstancesWithTheSameArrayElements()
        {
            var element1 = new object();
            var element2 = new object();
            var value1 = new[] { element1, element2 };
            var value2 = new[] { element1, element2 };
            var sut = InstanceFactory(value1);
            var other = InstanceFactory(value2);

            ClassicAssert.That(sut, Is.EqualTo(other));
        }

        [Test]
        public void TwoInstancesHaveTheSameHashCodeIfTheirValuesAreDifferentArrayInstancesWithTheSameArrayElements()
        {
            var element1 = new object();
            var element2 = new object();
            var value1 = new[] { element1, element2 };
            var value2 = new[] { element1, element2 };
            var sut = InstanceFactory(value1);
            var other = InstanceFactory(value2);

            ClassicAssert.That(sut.GetHashCode(), Is.EqualTo(other.GetHashCode()));
        }
    }

    [TestFixture]
    public class WithFilledReferenceTypeInstance : WithFilledInstance<object>
    {
        protected override object ValueFactory()
        {
            return new object();
        }

        protected override Optional<object> InstanceFactory(object value)
        {
            return new Optional<object>(value);
        }
    }

    [TestFixture]
    public class WithFilledValueTypeInstance : WithFilledInstance<Int32>
    {
        private readonly HashSet<int> _taken = new HashSet<int>();

        protected override int ValueFactory()
        {
            var next =
                new Random().
                    Next(Int32.MinValue, Int32.MaxValue);
            while (_taken.Contains(next))
                next = new Random().
                    Next(Int32.MinValue, Int32.MaxValue);
            _taken.Add(next);
            return next;
        }

        protected override Optional<int> InstanceFactory(int value)
        {
            return new Optional<int>(value);
        }
    }

    [TestFixture]
    public abstract class WithFilledInstance<T>
    {
        private Optional<T> _sut;
        private T _value;

        [SetUp]
        public void Setup()
        {
            _value = ValueFactory();
            _sut = InstanceFactory(_value);
        }

        protected abstract T ValueFactory();
        protected abstract Optional<T> InstanceFactory(T value);

        [Test]
        public void HasValueReturnsTrue()
        {
            ClassicAssert.That(_sut.HasValue, Is.True);
        }

        [Test]
        public void ValueReturnsInitializationValue()
        {
            ClassicAssert.That(_sut.Value, Is.EqualTo(_value));
        }

        [Test]
        public void TypedEnumerationReturnsInitializationValue()
        {
            ClassicAssert.That(_sut, Is.EquivalentTo(new[] { _value }));
        }

        [Test]
        public void ObjectEnumerationReturnsInitializationValue()
        {
            var sut = (IEnumerable) _sut;

            ClassicAssert.That(sut, Is.EquivalentTo(new object[] { _value }));
        }

        [Test]
        public void DoesNotEqualNullUsingEquals()
        {
            ClassicAssert.IsFalse(_sut.Equals(null));
        }

        [Test]
        public void DoesNotEqualNullUsingEqualityOperator()
        {
            ClassicAssert.IsFalse(_sut == null);
        }

        [Test]
        public void DoesDifferFromNullUsingInequalityOperator()
        {
            ClassicAssert.IsTrue(_sut != null);
        }

        [Test]
        public void DoesNotObjectEqualNull()
        {
            ClassicAssert.IsFalse(_sut.Equals((object) null));
        }

        [Test]
        public void DoesNotEqualObjectOfOtherType()
        {
            ClassicAssert.IsFalse(_sut.Equals(new object()));
        }

        [Test]
        public void DoesEqualItselfUsingEquals()
        {
            ClassicAssert.IsTrue(_sut.Equals(_sut));
        }

        [Test]
        public void DoesEqualItselfUsingEqualityOperator()
        {
            var @ref = _sut;
            ClassicAssert.IsTrue(_sut == @ref);
        }

        [Test]
        public void DoesNotDifferFromItselfUsingInequalityOperator()
        {
            var @ref = _sut;
            ClassicAssert.IsFalse(_sut != @ref);
        }

        [Test]
        public void DoesObjectEqualItself()
        {
            ClassicAssert.IsTrue(_sut.Equals((object) _sut));
        }

        [Test]
        public void TwoInstancesAreEqualIfTheyBothHaveTheSameValue()
        {
            ClassicAssert.IsTrue(_sut.Equals(InstanceFactory(_value)));
        }

        [Test]
        public void TwoInstancesAreEqualUsingEqualityOperatorIfTheyBothHaveTheSameValue()
        {
            ClassicAssert.IsTrue(_sut == InstanceFactory(_value));
        }

        [Test]
        public void TwoInstancesDoNotDifferUsingInequalityOperatorIfTheyBothHaveTheSameValue()
        {
            ClassicAssert.IsFalse(_sut != InstanceFactory(_value));
        }

        [Test]
        public void TwoInstancesAreNotEqualIfTheOtherHasNoValue()
        {
            ClassicAssert.IsFalse(_sut.Equals(Optional<T>.Empty));
        }

        [Test]
        public void TwoInstancesAreNotEqualUsingEqualityOperatorIfTheOtherHasNoValue()
        {
            ClassicAssert.IsFalse(_sut == Optional<T>.Empty);
        }

        [Test]
        public void TwoInstancesDoDifferUsingInequalityOperatorIfTheOtherHasNoValue()
        {
            ClassicAssert.IsTrue(_sut != Optional<T>.Empty);
        }

        [Test]
        public void TwoInstancesAreNotEqualIfTheOtherHasDifferentValue()
        {
            ClassicAssert.IsFalse(_sut.Equals(InstanceFactory(ValueFactory())));
        }

        [Test]
        public void TwoInstancesAreNotEqualUsingEqualityOperatorIfTheOtherHasDifferentValue()
        {
            ClassicAssert.IsFalse(_sut == InstanceFactory(ValueFactory()));
        }

        [Test]
        public void TwoInstancesDifferUsingTheInequalityOperatorIfTheOtherHasDifferentValue()
        {
            ClassicAssert.IsTrue(_sut != InstanceFactory(ValueFactory()));
        }

        [Test]
        public void TwoInstancesAreNotEqualIfTheOtherHasNoValueAndIsOfDifferentType()
        {
            ClassicAssert.IsFalse(_sut.Equals(Optional<MismatchObject>.Empty));
        }

        [Test]
        public void TwoInstancesAreNotEqualIfTheOtherHasDifferentValueAndIsOfDifferentType()
        {
            ClassicAssert.IsFalse(_sut.Equals(new Optional<MismatchObject>(new MismatchObject())));
        }

        [Test]
        public void TwoInstancesHaveTheSameHashCodeIfTheyBothHaveTheSameValue()
        {
            ClassicAssert.IsTrue(_sut.GetHashCode().Equals(InstanceFactory(_value).GetHashCode()));
        }

        [Test]
        public void TwoInstancesDoNotHaveTheSameHashCodeIfTheOtherHasNoValue()
        {
            ClassicAssert.IsFalse(_sut.GetHashCode().Equals(Optional<T>.Empty.GetHashCode()));
        }

        [Test]
        public void TwoInstancesDoNotHaveTheSameHashCodeIfTheOtherHasDifferentValue()
        {
            ClassicAssert.IsFalse(_sut.GetHashCode().Equals(InstanceFactory(ValueFactory()).GetHashCode()));
        }

        [Test]
        public void TwoInstancesDoNotHaveTheSameHashCodeIfTheOtherHasNoValueAndIsOfDifferentValueType()
        {
            ClassicAssert.IsFalse(_sut.GetHashCode().Equals(Optional<MismatchObject>.Empty.GetHashCode()));
        }

        [Test]
        public void TwoInstancesDoNotHaveTheSameHashCodeIfTheOtherHasDifferentValueAndIsOfDifferentValueType()
        {
            ClassicAssert.IsFalse(
                _sut.GetHashCode().Equals(new Optional<MismatchObject>(new MismatchObject()).GetHashCode()));
        }
    }

    internal class MismatchObject { }
}
