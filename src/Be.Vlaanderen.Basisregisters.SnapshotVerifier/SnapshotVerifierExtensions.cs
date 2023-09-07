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
        public static IServiceCollection AddSnapshotVerificationServices<TAggregate, TAggregateId>(
            this IServiceCollection services,
            string snapshotConnectionString,
            string snapshotSchema,
            string snapshotVerificationStatesTableName = "SnapshotVerificationStates")
            where TAggregate : class, IAggregateRootEntity, ISnapshotable
            where TAggregateId : class
        {
            services.AddSingleton(new MsSqlSnapshotStoreQueries(snapshotConnectionString, snapshotSchema));
            services.AddScoped<ISnapshotVerificationRepository>(_ => new SnapshotVerificationRepository(snapshotConnectionString, snapshotSchema, snapshotVerificationStatesTableName));
            services.AddScoped<IAggregateSnapshotRepository<TAggregate>, AggregateSnapshotRepository<TAggregate>>();
            services.AddScoped<IAggregateEventsRepository<TAggregate, TAggregateId>, AggregateEventsRepository<TAggregate, TAggregateId>>();
            return services;
        }

        public static IServiceCollection AddHostedSnapshotVerifierService<TAggregate, TAggregateId>(
            this IServiceCollection services,
            Func<TAggregate, TAggregateId> aggregateIdFactory,
            List<string> membersToIgnoreInVerification)
            where TAggregate : class, IAggregateRootEntity, ISnapshotable
            where TAggregateId : class
        {
            services.AddHostedService(x => new SnapshotVerifier<TAggregate, TAggregateId>(
                x.GetRequiredService<IHostApplicationLifetime>(),
                aggregateIdFactory,
                membersToIgnoreInVerification,
                x.GetRequiredService<ISnapshotVerificationRepository>(),
                x.GetRequiredService<IAggregateSnapshotRepository<TAggregate>>(),
                x.GetRequiredService<IAggregateEventsRepository<TAggregate, TAggregateId>>(),
                x.GetService<ISnapshotVerificationNotifier>(), x.GetRequiredService<ILoggerFactory>()));

            return services;
        }
    }
}
