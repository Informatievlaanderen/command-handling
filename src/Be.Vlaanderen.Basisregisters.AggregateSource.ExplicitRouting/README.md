AggregateSource ExplicitRouting
====================

Source:
- https://github.com/yreynhout/AggregateSource
- https://github.com/yreynhout/AggregateSource/tree/master/src/Core/AggregateSource.Content.ExplicitRouting

> DEV NOTE:
>
> This is a specific implementation of AggregateSource.

Requires you to register a callback handler for each event that changes the aggregate's state. This can also be seen as an advantage, you only have to register callback handlers for events that actually change the internal state of the aggregate. Those that don't need to change the internal state do not require a callback handler. Another advantage is that you can use an Action<TEvent> instead of a dedicated method as callback handler which slightly reduces the amount of code to read and type.

Source:
- https://github.com/yreynhout/AggregateSource
- https://github.com/yreynhout/AggregateSource/tree/master/src/Core/AggregateSource
