namespace Be.Vlaanderen.Basisregisters.SnapshotVerifier.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AggregateSource;
    using AggregateSource.Snapshotting;

    public class FakeAggregate : AggregateRootEntity, ISnapshotable
    {
        private readonly int _privateMember;
        public int PublicMember;

        private int PrivateProperty { get; set; }
        public int PublicProperty { get; set; }

        private readonly int _backingField;
        public int PublicPropertyWithBackingField => _backingField;

        private readonly IList<int> _backingListField;
        public IEnumerable<int> PublicPropertyWithBackingListFiltered => _backingListField.Skip(1).Take(1);

        public FakeAggregate(int privateMember,
            int publicMember,
            int privateProperty,
            int publicProperty,
            int backingField,
            IList<int> backingListField)
        {
            _privateMember = privateMember;
            PublicMember = publicMember;

            PrivateProperty = privateProperty;
            PublicProperty = publicProperty;

            _backingField = backingField;
            _backingListField = backingListField;

            Strategy = NoSnapshotStrategy.Instance;
        }

        public FakeAggregate WithDifferentPrivateMember(int value)
        {
            return new FakeAggregate(
                value,
                PublicMember,
                PrivateProperty,
                PublicProperty,
                _backingField,
                _backingListField);
        }

        public FakeAggregate WithDifferentPublicMember(int value)
        {
            return new FakeAggregate(
                _privateMember,
                value,
                PrivateProperty,
                PublicProperty,
                _backingField,
                _backingListField);
        }

        public FakeAggregate WithDifferentPrivateProperty(int value)
        {
            return new FakeAggregate(
                _privateMember,
                PublicMember,
                value,
                PublicProperty,
                _backingField,
                _backingListField);
        }

        public FakeAggregate WithDifferentPublicProperty(int value)
        {
            return new FakeAggregate(
                _privateMember,
                PublicMember,
                PrivateProperty,
                value,
                _backingField,
                _backingListField);
        }

        public FakeAggregate WithDifferentBackingField(int value)
        {
            return new FakeAggregate(
                _privateMember,
                PublicMember,
                PrivateProperty,
                PublicProperty,
                value,
                _backingListField);
        }

        public FakeAggregate WithDifferentBackingListField(IList<int> value)
        {
            return new FakeAggregate(
                _privateMember,
                PublicMember,
                PrivateProperty,
                PublicProperty,
                _backingField,
                value);
        }

        public ISnapshotStrategy Strategy { get; }

        public object TakeSnapshot()
        {
            throw new NotImplementedException();
        }
    }
}
