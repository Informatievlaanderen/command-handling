namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System;
    using System.Threading.Tasks;

    internal static class Catch
    {
        public static Optional<Exception> Exception(Action action)
        {
            var result = Optional<Exception>.Empty;

            try
            {
                action();
            }
            catch (Exception exception)
            {
                result = new Optional<Exception>(exception);
            }

            return result;
        }

        public static async Task<Optional<Exception>> Exception(Func<Task> action)
        {
            var result = Optional<Exception>.Empty;

            try
            {
                await action();
            }
            catch (Exception exception)
            {
                result = new Optional<Exception>(exception);
            }

            return result;
        }
    }
}
