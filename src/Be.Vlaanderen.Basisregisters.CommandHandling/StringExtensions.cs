namespace Be.Vlaanderen.Basisregisters.CommandHandling
{
    using System.Globalization;

    internal static class StringExtensions
    {
        internal static string FormatWith(this string format, params object[] args)
            => string.Format(CultureInfo.InvariantCulture, format, args);
    }
}
