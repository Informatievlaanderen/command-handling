namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System;
    using System.IO;
    using System.Linq;
    using AggregateSource;
    using Microsoft.Extensions.Logging;

#if NUNIT
    /// <summary>
    /// NUnit specific extension methods for asserting aggregate factory behavior.
    /// </summary>
    public static class NUnitExtensionsForTestSpecification
#elif XUNIT
    /// <summary>
    /// Xunit specific extension methods for asserting test specifications.
    /// </summary>
    public static class XunitExtensionsForTestSpecification
#endif
    {
        /// <summary>
        ///     Asserts that the specification is met.
        /// </summary>
        /// <param name="builder">The specification builder.</param>
        /// <param name="runner"></param>
        /// <param name="comparer">The event comparer.</param>
        /// <param name="logger">A logger.</param>
        public static void Assert(
            this IEventCentricTestSpecificationBuilder builder,
            IEventCentricTestSpecificationRunner runner,
            IFactComparer comparer,
            ILogger logger)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (runner == null)
                throw new ArgumentNullException(nameof(runner));

            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            var specification = builder.Build();
            logger.LogTrace($"Given: \r\n{specification.Givens.ToLogStringVerbose()}");
            logger.LogTrace($"When: \r\n{specification.When.ToLogString()}");
            logger.LogTrace($"Then: \r\n{specification.Thens.ToLogStringVerbose()}");

            var result = runner.Run(specification);

            if (result.Failed)
            {
                if (result.ButException.HasValue)
                {
                    logger.LogTrace($"Actual exception: \r\n{result.ButException.Value.ToLogString(includeStackTrace: true)}");

                    using (var writer = new StringWriter())
                    {
                        writer.WriteLine("  Expected: {0} event(s),", result.Specification.Thens.Length);
                        writer.WriteLine("  But was:  {0}", result.ButException.Value);

#if NUNIT
                        throw new NUnit.Framework.AssertionException(writer.ToString());
#elif XUNIT
                        throw new Xunit.Sdk.XunitException(writer.ToString());
#endif
                    }
                }

                if (result.ButEvents.HasValue)
                {
                    logger.LogTrace($"Actual events: \r\n{result.ButEvents.Value.ToLogStringVerbose()}");

                    if (result.ButEvents.Value.Length != result.Specification.Thens.Length)
                        using (var writer = new StringWriter())
                        {
                            writer.WriteLine("  Expected: {0} event(s) ({1}),",
                                result.Specification.Thens.Length,
                                result.Specification.Thens.ToLogStringShort());

                            writer.WriteLine("  But was:  {0} event(s) ({1})",
                                result.ButEvents.Value.Length,
                                result.ButEvents.Value.ToLogStringShort());

#if NUNIT
                            throw new NUnit.Framework.AssertionException(writer.ToString());
#elif XUNIT
                            throw new Xunit.Sdk.XunitException(writer.ToString());
#endif
                        }

                    using (var writer = new StringWriter())
                    {
                        writer.WriteLine("  Expected: {0} event(s) ({1}),",
                            result.Specification.Thens.Length,
                            result.Specification.Thens.ToLogStringShort());

                        writer.WriteLine("  But found the following differences:");
                        foreach (var difference in result.Specification.Thens
                            .Zip(result.ButEvents.Value, (expected, actual) => new Tuple<Fact, Fact>(expected, actual))
                            .SelectMany(_ => comparer.Compare(_.Item1, _.Item2)))
                            writer.WriteLine("    {0}", difference.Message);

#if NUNIT
                        throw new NUnit.Framework.AssertionException(writer.ToString());
#elif XUNIT
                        throw new Xunit.Sdk.XunitException(writer.ToString());
#endif
                    }
                }

                logger.LogTrace("Actual events: None");
            }

            logger.LogTrace(result.ButEvents.HasValue
                ? $"Actual events: \r\n{result.ButEvents.Value.ToLogStringVerbose()}"
                : "Actual events: None");
        }

        /// <summary>
        ///     Asserts that the specification is met.
        /// </summary>
        /// <param name="builder">The specification builder.</param>
        /// <param name="runner"></param>
        /// <param name="comparer">The exception comparer.</param>
        /// <param name="logger">A logger.</param>
        public static void Assert(
            this IExceptionCentricTestSpecificationBuilder builder,
            IExceptionCentricTestSpecificationRunner runner,
            IExceptionComparer comparer,
            ILogger logger)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (runner == null)
                throw new ArgumentNullException(nameof(runner));

            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            var specification = builder.Build();
            logger.LogTrace($"Given: \r\n{specification.Givens.ToLogStringVerbose()}");
            logger.LogTrace($"When: \r\n{specification.When.ToLogString()}");
            logger.LogTrace($"Throws: \r\n{specification.Throws.ToLogString(includeStackTrace: false)}");

            var result = runner.Run(specification);

            if (result.Failed)
            {
                if (result.ButException.HasValue)
                {
                    logger.LogTrace($"Actual exception: \r\n{result.ButException.Value.ToLogString(includeStackTrace: true)}");

                    if (result.ButException.Value.GetType() != result.Specification.Throws.GetType())
                        using (var writer = new StringWriter())
                        {
                            writer.WriteLine("  Expected: {0},", result.Specification.Throws);
                            writer.WriteLine("  But was:  {0}", result.ButException.Value);

#if NUNIT
                            throw new NUnit.Framework.AssertionException(writer.ToString());
#elif XUNIT
                            throw new Xunit.Sdk.XunitException(writer.ToString());
#endif
                        }

                    using (var writer = new StringWriter())
                    {
                        writer.WriteLine("  Expected: {0},", result.Specification.Throws);
                        writer.WriteLine("  But found the following differences:");

                        foreach (var difference in comparer.Compare(result.Specification.Throws, result.ButException.Value))
                            writer.WriteLine("    {0}", difference.Message);

#if NUNIT
                        throw new NUnit.Framework.AssertionException(writer.ToString());
#elif XUNIT
                        throw new Xunit.Sdk.XunitException(writer.ToString());
#endif
                    }
                }
                logger.LogTrace("Actual exception: None");

                if (result.ButEvents.HasValue)
                {
                    logger.LogTrace($"Actual events: \r\n{result.ButEvents.Value.ToLogStringVerbose()}");

                    using (var writer = new StringWriter())
                    {
                        writer.WriteLine("  Expected: {0},", result.Specification.Throws);
                        writer.WriteLine("  But was:  {0} event(s) ({1})",
                            result.ButEvents.Value.Length,
                            result.ButEvents.Value.ToLogStringShort());

#if NUNIT
                        throw new NUnit.Framework.AssertionException(writer.ToString());
#elif XUNIT
                        throw new Xunit.Sdk.XunitException(writer.ToString());
#endif
                    }
                }

                using (var writer = new StringWriter())
                {
                    writer.WriteLine("  Expected: {0},", result.Specification.Throws);
                    writer.WriteLine("  But no exception occurred");

#if NUNIT
                    throw new NUnit.Framework.AssertionException(writer.ToString());
#elif XUNIT
                    throw new Xunit.Sdk.XunitException(writer.ToString());
#endif
                }
            }

            if (result.ButException.HasValue)
                logger.LogTrace($"Actual exception: \r\n{result.ButException.Value.ToLogString(includeStackTrace: true)}");
        }
    }
}
