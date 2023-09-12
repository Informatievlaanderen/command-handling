namespace Be.Vlaanderen.Basisregisters.SnapshotVerifier.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using KellermanSoftware.CompareNetObjects;

    public static class ComparisonConfigExtensions
    {
        public static ComparisonConfig WithMemberToIgnore(this ComparisonConfig config, string memberToIgnore)
        {
            return new ComparisonConfig
            {
                MembersToIgnore = config.MembersToIgnore.Concat(new[] { memberToIgnore }).ToList(),
                ComparePrivateFields = config.ComparePrivateFields,
                CompareBackingFields = config.CompareBackingFields,
                ComparePrivateProperties = config.ComparePrivateProperties,
                IgnoreCollectionOrder = config.IgnoreCollectionOrder,
                CollectionMatchingSpec =  config.CollectionMatchingSpec
            };
        }

        public static ComparisonConfig WithMembersToIgnore(this ComparisonConfig config,
            IEnumerable<string> membersToIgnore)
        {
            return new ComparisonConfig
            {
                MembersToIgnore = config.MembersToIgnore.Concat(membersToIgnore).ToList(),
                ComparePrivateFields = config.ComparePrivateFields,
                CompareBackingFields = config.CompareBackingFields,
                ComparePrivateProperties = config.ComparePrivateProperties,
                IgnoreCollectionOrder = config.IgnoreCollectionOrder,
                CollectionMatchingSpec =  config.CollectionMatchingSpec
            };
        }

        public static ComparisonConfig WithCollectionMatchingSpec(this ComparisonConfig config,
            (Type, string) collectionMatchingSpec)
        {
            var dict = config.CollectionMatchingSpec
                .ToDictionary(x => x.Key, x => x.Value);
            dict.Add(collectionMatchingSpec.Item1, new[] { collectionMatchingSpec.Item2 });

            return new ComparisonConfig
            {
                MembersToIgnore = config.MembersToIgnore,
                ComparePrivateFields = config.ComparePrivateFields,
                CompareBackingFields = config.CompareBackingFields,
                ComparePrivateProperties = config.ComparePrivateProperties,
                IgnoreCollectionOrder = config.IgnoreCollectionOrder,
                CollectionMatchingSpec = dict
            };
        }
    }
}
