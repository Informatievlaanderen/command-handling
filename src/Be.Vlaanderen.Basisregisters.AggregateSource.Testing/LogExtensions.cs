namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using AggregateSource;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public static class LogExtensions
    {
        public static JsonSerializerSettings LogJsonSerializerSettings => new JsonSerializerSettings();

        public static string ToLogStringShort(this Fact[] facts)
            => string.Join(
                Environment.NewLine,
                facts
                    .GroupBy(f => f.Identifier)
                    .Select(group => $"{group.Key}: {string.Join(", ", group.Select(f => f.Event.GetType().Name))}"));

        public static string ToLogStringVerbose(this Fact[] facts, Formatting formatting = Formatting.Indented)
            => facts.Select(f => new { f.Identifier, Event = f.Event.ToAnonymousWithTypeInfo() }).ToLogStringLimited(formatting, int.MaxValue);

        public static string ToLogStringLimited<T>(this IEnumerable<T> objects, Formatting formatting = Formatting.Indented, int max = 5)
        {
            var objectsList = objects.ToList();
            return objectsList.Count < max ? JsonConvert.SerializeObject(objectsList.Select(o => o?.ToAnonymousWithTypeInfo()), formatting, LogJsonSerializerSettings) : "...";
        }

        public static string ToLogString<T>(this T @object, Formatting formatting = Formatting.Indented)
            => JsonConvert.SerializeObject(@object?.ToAnonymousWithTypeInfo(), formatting, LogJsonSerializerSettings);

        public static string ToLogString(this Exception exception, Formatting formatting = Formatting.Indented, bool includeStackTrace = false)
        {
            return includeStackTrace
                ? JsonConvert.SerializeObject(new { _type = exception.GetType().Name, exception.Message }, formatting, LogJsonSerializerSettings)
                : JsonConvert.SerializeObject(new { _type = exception.GetType().Name, exception.Message, exception.StackTrace }, formatting, LogJsonSerializerSettings);
        }

        public static dynamic ToAnonymousWithTypeInfo(this object @object)
        {
            return @object.GetType().IsOfTypeAnonymous()
                ? @object
                : new { _type = @object.GetType().Name, _payload = @object };
        }

        private static bool IsOfTypeAnonymous(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            // HACK: The only way to detect anonymous types right now.
            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false) &&
                   type.IsGenericType && type.Name.Contains("AnonymousType") &&
                   (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$")) &&
                   (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
        }
    }
}
