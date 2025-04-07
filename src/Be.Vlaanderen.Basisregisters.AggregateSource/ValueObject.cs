namespace Be.Vlaanderen.Basisregisters.AggregateSource
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Newtonsoft.Json;
    using NodaTime;
    using NodaTime.Text;
    using Utilities;
    using Utilities.HexByteConvertor;

    public abstract class GuidValueObject<T> : StructDataTypeValueObject<T, Guid>, IComparable where T : GuidValueObject<T>
    {
        protected GuidValueObject(Guid id) : base(id) { }

        public override string ToString() => Value.ToString("D", CultureInfo.InvariantCulture);

        int IComparable.CompareTo(object obj) => Value.CompareTo(((GuidValueObject<T>)obj).Value);
    }

    public abstract class BooleanValueObject<T> : StructDataTypeValueObject<T, bool>, IComparable where T : BooleanValueObject<T>
    {
        protected BooleanValueObject(bool boolean) : base(boolean) { }

        public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

        int IComparable.CompareTo(object obj) => Value.CompareTo(((BooleanValueObject<T>)obj).Value);
    }

    public abstract class DateTimeValueObject<T> : StructDataTypeValueObject<T, DateTime>, IComparable where T : DateTimeValueObject<T>
    {
        protected DateTimeValueObject(DateTime date) : base(date) { }

        public override string ToString() => Value.ToString("O", CultureInfo.InvariantCulture);

        int IComparable.CompareTo(object obj) => Value.CompareTo(((DateTimeValueObject<T>)obj).Value);
    }

    public abstract class DateTimeOffsetValueObject<T> : StructDataTypeValueObject<T, DateTimeOffset>, IComparable where T : DateTimeOffsetValueObject<T>
    {
        protected DateTimeOffsetValueObject(DateTimeOffset date) : base(date) { }

        public override string ToString() => Value.ToString("O", CultureInfo.InvariantCulture);

        int IComparable.CompareTo(object obj) => Value.CompareTo(((DateTimeOffsetValueObject<T>)obj).Value);
    }

    public abstract class InstantValueObject<T> : StructDataTypeValueObject<T, Instant>, IComparable where T : InstantValueObject<T>
    {
        protected InstantValueObject(Instant instant) : base(instant) { }

        public override string ToString() => Value.ToString(InstantPattern.ExtendedIso.PatternText, CultureInfo.InvariantCulture);

        int IComparable.CompareTo(object obj) => Value.CompareTo(((InstantValueObject<T>)obj).Value);
    }

    public abstract class OffsetDateTimeValueObject<T> : StructDataTypeValueObject<T, OffsetDateTime>, IComparable where T : OffsetDateTimeValueObject<T>
    {
        protected OffsetDateTimeValueObject(OffsetDateTime offsetDateTime) : base(offsetDateTime) { }

        public override string ToString() => Value.ToString("o", CultureInfo.InvariantCulture);

        int IComparable.CompareTo(object obj) => Value.Minus(((OffsetDateTimeValueObject<T>)obj).Value).TotalTicks.CompareTo(0d);
    }

    public abstract class ZonedDateTimeValueObject<T> : StructDataTypeValueObject<T, ZonedDateTime>, IComparable where T : ZonedDateTimeValueObject<T>
    {
        protected ZonedDateTimeValueObject(ZonedDateTime zonedDateTime) : base(zonedDateTime) { }

        public override string ToString() => Value.ToString("F", CultureInfo.InvariantCulture);

        int IComparable.CompareTo(object obj) => Value.Minus(((ZonedDateTimeValueObject<T>)obj).Value).TotalTicks.CompareTo(0d);
    }
    public abstract class LocalDateTimeValueObject<T> : StructDataTypeValueObject<T, LocalDateTime>, IComparable where T : LocalDateTimeValueObject<T>
    {
        protected LocalDateTimeValueObject(LocalDateTime localDateTime) : base(localDateTime) { }

        public override string ToString() => Value.ToString("R", CultureInfo.InvariantCulture);

        int IComparable.CompareTo(object obj) => Value.CompareTo(((LocalDateTimeValueObject<T>)obj).Value);
    }

    public abstract class StringValueObject<T> : ClassDataTypeValueObject<T, string>, IComparable where T : StringValueObject<T>
    {
        protected StringValueObject(string @string) : base(@string) { }

        public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

        int IComparable.CompareTo(object obj) => string.CompareOrdinal(Value, ((StringValueObject<T>)obj).Value);
    }

    public abstract class IntegerValueObject<T> : StructDataTypeValueObject<T, int>, IComparable where T : IntegerValueObject<T>
    {
        protected IntegerValueObject(int number) : base(number) { }

        public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

        int IComparable.CompareTo(object obj) => Value.CompareTo(((IntegerValueObject<T>)obj).Value);
    }

    public abstract class DoubleValueObject<T> : StructDataTypeValueObject<T, double>, IComparable where T : DoubleValueObject<T>
    {
        protected DoubleValueObject(double number) : base(number) { }

        public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

        int IComparable.CompareTo(object obj) => Value.CompareTo(((DoubleValueObject<T>)obj).Value);
    }

    public abstract class DecimalValueObject<T> : StructDataTypeValueObject<T, decimal>, IComparable where T : DecimalValueObject<T>
    {
        protected DecimalValueObject(decimal number) : base(number) { }

        public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

        int IComparable.CompareTo(object obj) => Value.CompareTo(((DecimalValueObject<T>)obj).Value);
    }

    public abstract class ByteArrayValueObject<T> : ClassDataTypeValueObject<T, byte[]> where T : ByteArrayValueObject<T>
    {
        protected ByteArrayValueObject(byte[] bytes) : base(bytes) { }

        public override string? ToString() => Value.ToHexString();

        protected override IEnumerable<object> Reflect() => Value.Cast<object>();
    }

    public abstract class ClassDataTypeValueObject<T, TDataType> : ValueObject<T>where T : ValueObject<T> where TDataType : class
    {
        [JsonProperty]
        protected TDataType Value { get; }

        [JsonConstructor]
        protected ClassDataTypeValueObject(TDataType value) => Value = value;

        public static implicit operator TDataType(ClassDataTypeValueObject<T, TDataType> valueObject) => valueObject.Value;

        protected override IEnumerable<object> Reflect()
        {
            yield return Value;
        }
    }

    public abstract class StructDataTypeValueObject<T, TDataType> : ValueObject<T> where T : ValueObject<T> where TDataType : struct
    {
        [JsonProperty]
        protected TDataType Value { get; }

        [JsonConstructor]
        protected StructDataTypeValueObject(TDataType value) => Value = value;

        public static implicit operator TDataType(StructDataTypeValueObject<T, TDataType> valueObject) => valueObject.Value;
        public static implicit operator TDataType?(StructDataTypeValueObject<T, TDataType> valueObject) => valueObject?.Value;

        protected override IEnumerable<object> Reflect()
        {
            yield return Value;
        }
    }

    // https://smellegantcode.wordpress.com/2009/06/09/generic-value-object-with-yield-return/
    [Serializable]
    public abstract class ValueObject<T> : IEquatable<T> where T : ValueObject<T>
    {
        protected abstract IEnumerable<object> Reflect();

        public static implicit operator string?(ValueObject<T> valueObject)
        {
            return valueObject.ToString();
        }

        public static bool operator ==(ValueObject<T>? obj1, ValueObject<T>? obj2)
        {
            if (ReferenceEquals(obj1, obj2))
                return true;

            if (obj1 is null)
                return false;

            if (obj2 is null)
                return false;

            return obj1.Equals(obj2);
        }

        public static bool operator !=(ValueObject<T> obj1, ValueObject<T> obj2) => !(obj1 == obj2);

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != GetType())
                return false;

            return Equals(obj as T);
        }

        public bool Equals(T? other)
        {
            if (other is null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Reflect().SequenceEqual(other.Reflect());
        }

        public override int GetHashCode() => HashCodeCalculator.GetHashCode(Reflect());

        public override string? ToString() => ToStringBuilder.ToString(Reflect());
    }
}
