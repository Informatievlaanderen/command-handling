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

        /// <summary>
        /// Add the snapshot verifier in a hosted service.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="aggregateFactory"></param>
        /// <param name="aggregateIdFactory"></param>
        /// <param name="membersToIgnoreInVerification"></param>
        /// <param name="collectionMatchingSpec">
        /// Sometimes one wants to match items between collections by some key first, and then compare the matched objects.
        /// Without this, the comparer basically says there is no match in collection B for any given item in collection A that doesn't Compare with a result of true.
        /// </param>
        /// <typeparam name="TAggregateRoot"></typeparam>
        /// <typeparam name="TStreamId"></typeparam>
        /// <returns></returns>
        public static IServiceCollection AddHostedSnapshotVerifierService<TAggregateRoot, TStreamId>(
            this IServiceCollection services,
            Func<TAggregateRoot> aggregateFactory,
            Func<TAggregateRoot, TStreamId> aggregateIdFactory,
            List<string> membersToIgnoreInVerification,
            Dictionary<Type, IEnumerable<string>> collectionMatchingSpec)
            where TAggregateRoot : class, IAggregateRootEntity, ISnapshotable
            where TStreamId : class
        {
            services.AddHostedService(x => new SnapshotVerifier<TAggregateRoot, TStreamId>(
                x.GetRequiredService<IHostApplicationLifetime>(),
                aggregateIdFactory,
                membersToIgnoreInVerification,
                collectionMatchingSpec,
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
