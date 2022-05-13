namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class EventCentricTestSpecificationRunner : IEventCentricTestSpecificationRunner
    {
        private readonly IExpectedFactComparer _comparer;
        private readonly IExpectedFactWriter _factWriter;
        private readonly IExpectedFactReader _factReader;
        private readonly IHandlerResolver _handlerResolver;

        public EventCentricTestSpecificationRunner(IExpectedFactComparer comparer, IExpectedFactWriter factWriter, IExpectedFactReader factReader, IHandlerResolver handlerResolver)
        {
            _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            _factWriter = factWriter ?? throw new ArgumentNullException(nameof(factWriter));
            _factReader = factReader ?? throw new ArgumentNullException(nameof(factReader));
            _handlerResolver = handlerResolver ?? throw new ArgumentNullException(nameof(handlerResolver));
        }

        public EventCentricTestResult Run(EventCentricTestSpecification specification)
            => RunAsync(specification).GetAwaiter().GetResult();

        private async Task<EventCentricTestResult> RunAsync(EventCentricTestSpecification spec)
        {
            var position = await _factWriter.PersistFacts(spec.Givens);

            var handleCommand = _handlerResolver.ResolveHandlerFor(spec.When);
            var result = await Catch.Exception(async () => await handleCommand(spec.When));

            if (result.HasValue)
            {
                return spec.Fail(result.Value);
            }

            var actualEvents = await _factReader.RetrieveFacts(position);

            return actualEvents.SequenceEqual(spec.Thens, new WrappedExpectedFactComparerEqualityComparer(_comparer))
                ? spec.Pass(actualEvents)
                : spec.Fail(actualEvents);
        }
    }
}
