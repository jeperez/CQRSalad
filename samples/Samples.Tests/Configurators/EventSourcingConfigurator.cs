﻿using CQRSalad.Domain;
using CQRSalad.EventSourcing;
using CQRSalad.EventStore.Core;
using CQRSalad.Infrastructure;
using StructureMap;

namespace Samples.Tests.Configurators
{
    public static class EventSourcingConfigurator
    {
        public static IContainer UseCommandProcessorSingleton(this IContainer container)
        {
            //container.Configure(expression => expression.For(typeof(IAggregateRepository<>)).Use(typeof(ShapshotAggregateRepository<>)).Ctor<int>().Is(2).Singleton());
            container.Configure(expression => expression.For(typeof(IAggregateRepository<>)).Use(typeof(AggregateRepository<>)).Singleton());
            return container;
        }

        public static IContainer UseInMemoryEventStore(this IContainer container)
        {
            container.Configure(expression => expression.For(typeof(IEventStore)).Use(typeof(InMemoryEventStore)).Singleton());
            container.Configure(expression => expression.For(typeof(IEventBus)).Use(typeof(InMemoryEventBus)).Singleton());
            return container;
        }

        public static IContainer UseMongoEventStore(this IContainer container, string connectionString)
        {
            //var mongoEvents = new MongoInstance(connectionString);
            //var eventStore = new StreamBasedEventStore(mongoEvents.GetDatabase(), EventStoreSettings.GetDefault());
            //container.Configure(expression => expression.For<IEventStore>().Use(eventStore).Singleton());

            //var snapshotsStore = new MongoSnapshotStore(mongoEvents.GetDatabase(), new MongoSnapshotsOptions() { CollectionName = "snapshots"});
            //container.Configure(expression => expression.For<ISnapshotStore>().Use(snapshotsStore).Singleton());

            return container;
        }
    }
}