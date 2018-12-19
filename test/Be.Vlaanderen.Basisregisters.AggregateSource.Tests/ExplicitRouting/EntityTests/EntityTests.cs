namespace Be.Vlaanderen.Basisregisters.AggregateSource.Tests.ExplicitRouting.EntityTests
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class WithAnyInstance
    {
        [Test]
        public void IsInstanceEventRouter()
        {
            Assert.That(new AnyInstanceEntity(), Is.InstanceOf<IInstanceEventRouter>());
        }

        [Test]
        public void ApplierCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new UseNullApplierEntity());
        }

        [Test]
        public void RouteEventCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RouteWithNullEventEntity());
        }

        [Test]
        public void ApplyEventCannotBeNull()
        {
            var sut = new ApplyNullEventEntity();
            Assert.Throws<ArgumentNullException>(sut.ApplyNull);
        }

        [Test]
        public void RegisterHandlerCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RegisterNullHandlerEntity());
        }

        [Test]
        public void RegisterHandlerCanOnlyBeCalledOncePerEventType()
        {
            Assert.Throws<ArgumentException>(() => new RegisterSameEventHandlerTwiceEntity());
        }
    }

    internal class AnyInstanceEntity : Entity
    {
        public AnyInstanceEntity() : base(_ => { })
        {
        }
    }

    internal class UseNullApplierEntity : Entity
    {
        public UseNullApplierEntity() : base(null) { }
    }

    internal class RouteWithNullEventEntity : Entity
    {
        public RouteWithNullEventEntity() : base(_ => { })
        {
            Route(null);
        }
    }

    internal class ApplyNullEventEntity : Entity
    {
        public ApplyNullEventEntity() : base(_ => { }) { }

        public void ApplyNull()
        {
            Apply(null);
        }
    }

    internal class RegisterNullHandlerEntity : Entity
    {
        public RegisterNullHandlerEntity() : base(_ => { })
        {
            Register<object>(null);
        }
    }

    internal class RegisterSameEventHandlerTwiceEntity : Entity
    {
        public RegisterSameEventHandlerTwiceEntity() : base(_ => { })
        {
            Register<object>(o => { });
            Register<object>(o => { });
        }
    }

    [TestFixture]
    public class WithInstanceWithHandlers
    {
        WithHandlersEntity _sut;
        Action<object> _applier;
        List<object> _appliedEvents;

        [SetUp]
        public void Setup()
        {
            _appliedEvents = new List<object>();
            _applier = _ => _appliedEvents.Add(_);
            _sut = new WithHandlersEntity(_applier);
        }

        [Test]
        public void RouteCallsHandlerOfEvent()
        {
            var expectedEvent = new object();

            _sut.Route(expectedEvent);

            Assert.That(_sut.HandlerCallCount, Is.EqualTo(1));
            Assert.That(_sut.RoutedEvents, Is.EquivalentTo(new[] { expectedEvent }));
        }

        [Test]
        public void ApplyEventCallsApplier()
        {
            var @event = new object();

            _sut.DoApply(@event);

            Assert.That(_appliedEvents, Is.EquivalentTo(new[] { @event }));
        }
    }

    internal class WithHandlersEntity : Entity
    {
        public WithHandlersEntity(Action<object> applier)
            : base(applier)
        {
            RoutedEvents = new List<object>();
            Register<object>(@event =>
            {
                HandlerCallCount++;
                RoutedEvents.Add(@event);
            });
        }

        public void DoApply(object @event)
        {
            Apply(@event);
        }

        public int HandlerCallCount { get; private set; }
        public List<object> RoutedEvents { get; private set; }
    }

    [TestFixture]
    public class WithInstanceWithoutHandlers
    {
        WithoutHandlersEntity _sut;
        Action<object> _applier;
        List<object> _appliedEvents;

        [SetUp]
        public void Setup()
        {
            _appliedEvents = new List<object>();
            _applier = _ => _appliedEvents.Add(_);
            _sut = new WithoutHandlersEntity(_applier);
        }

        [Test]
        public void RouteDoesNotThrow()
        {
            Assert.DoesNotThrow(() => _sut.Route(new object()));
        }

        [Test]
        public void ApplyEventDoesNotThrow()
        {
            var @event = new object();

            _sut.DoApply(@event);

            Assert.That(_appliedEvents, Is.EquivalentTo(new[] { @event }));
        }
    }

    internal class WithoutHandlersEntity : Entity
    {
        public WithoutHandlersEntity(Action<object> applier) : base(applier) { }

        public void DoApply(object @event)
        {
            Apply(@event);
        }
    }
}
