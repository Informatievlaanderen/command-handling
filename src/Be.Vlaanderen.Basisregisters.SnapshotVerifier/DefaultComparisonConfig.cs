namespace Be.Vlaanderen.Basisregisters.SnapshotVerifier
{
    using System.Collections.Generic;
    using KellermanSoftware.CompareNetObjects;

    public sealed class DefaultComparisonConfig
    {
        public static ComparisonConfig Get => new()
        {
            MembersToIgnore = new List<string> { "_recorder", "_router", "_applier", "_lastEvent", "Strategy" },
            ComparePrivateFields = true,
            CompareBackingFields = false, // ONLY ignores compiler-generated backing fields.
            ComparePrivateProperties = true,
            IgnoreCollectionOrder = true,
        };
    }
}
