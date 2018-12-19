namespace Be.Vlaanderen.Basisregisters.AggregateSource.SqlStreamStore.Tests.Framework
{
    public class EventStub
    {
        public int Value { get; set; }

        public EventStub() {}

        public EventStub(int value) => Value = value;

        public override bool Equals(object obj) => Equals(obj as EventStub);

        private bool Equals(EventStub @event) => !ReferenceEquals(@event, null) && Value.Equals(@event.Value);

        public override int GetHashCode()
        {
            unchecked
            {
                return Value.GetHashCode()*10 + 2;
            }
        }
    }
}
