namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Constructor
{
    using System;

    internal class AggregateConstructorThrowStateBuilder : IAggregateConstructorThrowStateBuilder
	{
	    private readonly Func<IAggregateRootEntity> _sutFactory;
	    private readonly Exception _throws;

		public AggregateConstructorThrowStateBuilder(Func<IAggregateRootEntity> sutFactory, Exception throws)
		{
			_sutFactory = sutFactory;
			_throws = throws;
		}

		public ExceptionCentricAggregateConstructorTestSpecification Build()
		    => new ExceptionCentricAggregateConstructorTestSpecification(_sutFactory, _throws);
	}
}
