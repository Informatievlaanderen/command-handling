namespace Be.Vlaanderen.Basisregisters.SnapshotVerifier
{
    using System;
    using System.Collections.Generic;
    using AggregateSource;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public static class SnapshotVerifierExtensions
    {
        public static IServiceCollection AddSnapshotVerificationServices<TAggregateRoot, TStreamId>(
            this IServiceCollection services,
            string snapshotConnectionString,
            string snapshotSchema,
            string snapshotVerificationStatesTableName = "SnapshotVerificationStates")
            where TAggregateRoot : class, IAggregateRootEntity, ISnapshotable
            where TStreamId : class
        {
            services.AddSingleton(new MsSqlSnapshotStoreQueries(snapshotConnectionString, snapshotSchema));
            services.AddScoped<ISnapshotVerificationRepository>(_ => new SnapshotVerificationRepository(snapshotConnectionString, snapshotSchema, snapshotVerificationStatesTableName));
            services.AddScoped<IAggregateSnapshotRepository<TAggregateRoot>, AggregateSnapshotRepository<TAggregateRoot>>();
            services.AddScoped<IAggregateEventsRepository<TAggregateRoot, TStreamId>, AggregateEventsRepository<TAggregateRoot, TStreamId>>();
            return services;
        }

        public static IServiceCollection AddHostedSnapshotVerifierService<TAggregateRoot, TStreamId>(
            this IServiceCollection services,
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
                x.GetRequiredService<IAggregateSnapshotRepository<TAggregateRoot>>(),
                x.GetRequiredService<IAggregateEventsRepository<TAggregateRoot, TStreamId>>(),
                x.GetService<ISnapshotVerificationNotifier>(), x.GetRequiredService<ILoggerFactory>()));

            return services;
        }
    }
}
