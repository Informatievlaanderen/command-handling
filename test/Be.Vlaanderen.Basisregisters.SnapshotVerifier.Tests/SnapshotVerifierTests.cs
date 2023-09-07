namespace Be.Vlaanderen.Basisregisters.SnapshotVerifier.Tests
{
    using System;
    using System.Collections.Generic;
    using AggregateSource;
    using AggregateSource.Snapshotting;
    using AggregateSource.Testing.SqlStreamStore.Autofac;
    using Autofac;
    using Xunit;
    using Xunit.Abstractions;

    public class SnapshotVerifierTests : AutofacBasedTest
    {
        [Fact]
        public void ShouldCreateValidSnapshots()
        {

        }

        public SnapshotVerifierTests(ITestOutputHelper testOutputHelper, Action<ContainerBuilder> registerFunc = null) :
            base(testOutputHelper, registerFunc)
        {
        }

        protected override void ConfigureCommandHandling(ContainerBuilder builder)
        {
            throw new NotImplementedException();
        }

        protected override void ConfigureEventHandling(ContainerBuilder builder)
        {
            throw new NotImplementedException();
        }
    }

    /*private member
private property
public member
public property
private property die gebaseerd is op private member (private int _s; private int S => _s)
public property die gebaseerd is op private member (private int _s; private int S => _s)*/
    public class TestAggregate : AggregateRootEntity, ISnapshotable
    {
        private string _privateMember = "Lorem Ipsum";
        public string PublicMember = "Ipsum Lorem";

        private int _privateProperty { get; set; }
        public int PublicProperty { get; set; }

        private string name;

        public string Name

        {

            get { return name; }

            set { name = value; }

        }

        private string status;

        public string Status

        {

            get { return status; }

            set { status = value; }

        }

        public TestAggregate(int privateProperty, int publicProperty)
        {
            _privateProperty = privateProperty;
            PublicProperty = publicProperty;
        }


        public ISnapshotStrategy Strategy { get; }

        public object TakeSnapshot()
        {
            throw new NotImplementedException();
        }
    }

    public class TestAggregateStreamId : ValueObject<TestAggregateStreamId>
    {
        private readonly string _id;

        public TestAggregateStreamId(string id)
        {
            _id = id;
        }

        protected override IEnumerable<object> Reflect()
        {
            yield return _id;
        }

        public override string ToString() => $"testaggregate-{_id}";
    }
}
