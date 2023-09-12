namespace Be.Vlaanderen.Basisregisters.SnapshotVerifier
{
    using System.Collections.Generic;
    using KellermanSoftware.CompareNetObjects;
    using KellermanSoftware.CompareNetObjects.TypeComparers;

    public sealed class DefaultComparisonConfig
    {
        public static ComparisonConfig Instance => new()
        {
            MembersToIgnore = new List<string> { "_recorder", "_router", "_applier", "_lastEvent", "Strategy" },
            ComparePrivateFields = true,
            CompareBackingFields = false, // ONLY ignores compiler-generated backing fields.
            ComparePrivateProperties = true,
            IgnoreCollectionOrder = true,
            CustomComparers = new List<BaseTypeComparer>
            {
                new CustomListComparer(RootComparerFactory.GetRootComparer())
            }
        };
    }
}
