namespace Be.Vlaanderen.Basisregisters.SnapshotVerifier
{
    using System;
    using System.Collections.Generic;
    using AggregateSource;
    using EventHandling;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using SqlStreamStore;

    public static class SnapshotVerifierExtensions
    {
        public static IServiceCollection AddSnapshotVerificationServices(
            this IServiceCollection services,
            string snapshotConnectionString,
            string snapshotSchema,
            string snapshotVerificationStatesTableName = "SnapshotVerificationStates")
        {
            services.AddSingleton(new MsSqlSnapshotStoreQueries(snapshotConnectionString, snapshotSchema));
            services.AddScoped<ISnapshotVerificationRepository>(_ => new SnapshotVerificationRepository(snapshotConnectionString, snapshotSchema, snapshotVerificationStatesTableName));
            return services;
        }

        public static IServiceCollection AddHostedSnapshotVerifierService<TAggregateRoot, TStreamId>(
            this IServiceCollection services,
            Func<TAggregateRoot> aggregateFactory,
            Func<TAggregateRoot, TStreamId> aggregateIdFactory,
            List<string> membersToIgnoreInVerification)
            where TAggregateRoot : class, IAggregateRootEntity, ISnapshotable
            where TStreamId : class
        {
            services.AddHostedService(x => new SnapshotVerifier<TAggregateRoot, TStreamId>(
                x.GetRequiredService<IHostApplicationLifetime>(),
                aggregateIdFactory,
                membersToIgnoreInVerification,
                x.GetRequiredService<ISnapshotVerificationRepository>(),
                new AggregateSnapshotRepository<TAggregateRoot>(
                    x.GetRequiredService<MsSqlSnapshotStoreQueries>(),
                    x.GetRequiredService<EventDeserializer>(),
                    x.GetRequiredService<EventMapping>(),
                    aggregateFactory,
                    x.GetRequiredService<ILoggerFactory>()),
                new AggregateEventsRepository<TAggregateRoot, TStreamId>(
                    x.GetRequiredService<EventDeserializer>(),
                    x.GetRequiredService<EventMapping>(),
                    x.GetRequiredService<IReadonlyStreamStore>(),
                    aggregateFactory),
                x.GetService<ISnapshotVerificationNotifier>(), x.GetRequiredService<ILoggerFactory>()));

            return services;
        }
    }
}
