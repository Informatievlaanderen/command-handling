namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Query
{
    using System;

    internal class AggregateQueryWhenStateBuilder<TResult> : IAggregateQueryWhenStateBuilder<TResult>
    {
        private readonly Func<IAggregateRootEntity> _sutFactory;
        private readonly object[] _givens;
        private readonly Func<IAggregateRootEntity, object> _when;

        public AggregateQueryWhenStateBuilder(
            Func<IAggregateRootEntity> sutFactory,
            object[] givens,
            Func<IAggregateRootEntity, object> when)
        {
            _sutFactory = sutFactory;
            _givens = givens;
            _when = when;
        }

        public IAggregateQueryThenStateBuilder Then(TResult result) => new AggregateQueryThenStateBuilder(_sutFactory, _givens, _when, result);

        public IAggregateQueryThrowStateBuilder Throws(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            return new AggregateQueryThrowStateBuilder(_sutFactory, _givens, _when, exception);
        }
    }
}
