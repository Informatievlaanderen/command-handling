namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class ExceptionCentricTestSpecificationRunner : IExceptionCentricTestSpecificationRunner
    {
        private readonly IExceptionComparer _comparer;
        private readonly IFactWriter _factWriter;
        private readonly IFactReader _factReader;
        private readonly IHandlerResolver _handlerResolver;

        public ExceptionCentricTestSpecificationRunner(IExceptionComparer comparer, IFactWriter factWriter, IFactReader factReader, IHandlerResolver handlerResolver)
        {
            _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            _factWriter = factWriter ?? throw new ArgumentNullException(nameof(factWriter));
            _factReader = factReader ?? throw new ArgumentNullException(nameof(factReader));
            _handlerResolver = handlerResolver ?? throw new ArgumentNullException(nameof(handlerResolver));
        }

        public ExceptionCentricTestResult Run(ExceptionCentricTestSpecification specification)
            => RunAsync(specification).GetAwaiter().GetResult();

        private async Task<ExceptionCentricTestResult> RunAsync(ExceptionCentricTestSpecification spec)
        {
            var position = await _factWriter.PersistFacts(spec.Givens);

            var handleCommand = _handlerResolver.ResolveHandlerFor(spec.When);
            var result = await Catch.Exception(async () => await handleCommand(spec.When));

            var actualEvents = await _factReader.RetrieveFacts(position);

            if (!result.HasValue)
                return actualEvents.Any() ? spec.Fail(actualEvents) : spec.Fail();

            var actualException = result.Value;

            return _comparer.Compare(spec.Throws, actualException).Any()
                ? spec.Fail(actualException)
                : spec.Pass(actualException);
        }
    }
}
