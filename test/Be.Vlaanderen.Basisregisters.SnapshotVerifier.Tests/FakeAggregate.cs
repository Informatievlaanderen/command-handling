﻿namespace Be.Vlaanderen.Basisregisters.SnapshotVerifier.Tests
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

        private readonly IList<FakeEntity> _backingListField;
        public IEnumerable<FakeEntity> PublicPropertyWithBackingListFiltered => _backingListField.Skip(1).Take(1);

        public List<FakeEntity> List { get; set; }

        public FakeAggregate(
            int privateMember,
            int publicMember,
            int privateProperty,
            int publicProperty,
            int backingField,
            IList<FakeEntity> backingListField)
        {
            _privateMember = privateMember;
            PublicMember = publicMember;
            _backingField = backingField;
            _backingListField = backingListField;
            PrivateProperty = privateProperty;
            PublicProperty = publicProperty;
            Strategy = NoSnapshotStrategy.Instance;
        }

        public FakeAggregate(
            int privateMember,
            int publicMember,
            int privateProperty,
            int publicProperty,
            int backingField,
            IList<int> backingListField)
            : this(privateMember, publicMember, privateProperty, publicProperty, backingField,
                backingListField.Select(x => new FakeEntity(x)).ToList())
        {
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

        public FakeAggregate WithDifferentBackingListField(IList<FakeEntity> value)
        {
            return new FakeAggregate(
                _privateMember,
                PublicMember,
                PrivateProperty,
                PublicProperty,
                _backingField,
                value);
        }

        public FakeAggregate WithDifferentList(IEnumerable<FakeEntity> value)
        {
            return new FakeAggregate(
                _privateMember,
                PublicMember,
                PrivateProperty,
                PublicProperty,
                _backingField,
                _backingListField)
            {
                List = value.ToList()
            };
        }

        public ISnapshotStrategy Strategy { get; }

        public object TakeSnapshot()
        {
            throw new NotImplementedException();
        }
    }
}
