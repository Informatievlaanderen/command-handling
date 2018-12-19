namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Constructor
{
    using System;
    using System.Linq;

    internal class AggregateConstructorThenStateBuilder : IAggregateConstructorThenStateBuilder
	{
	    private readonly Func<IAggregateRootEntity> _sutFactory;
	    private readonly object[] _thens;

		public AggregateConstructorThenStateBuilder(Func<IAggregateRootEntity> sutFactory, object[] thens)
		{
			_sutFactory = sutFactory;
			_thens = thens;
		}

		public IAggregateConstructorThenStateBuilder Then(params object[] events)
		{
			if (events == null)
			    throw new ArgumentNullException(nameof(events));

            return new AggregateConstructorThenStateBuilder(_sutFactory, _thens.Concat(events).ToArray());
		}

		public EventCentricAggregateConstructorTestSpecification Build()
		    => new EventCentricAggregateConstructorTestSpecification(_sutFactory, _thens);
	}
}
