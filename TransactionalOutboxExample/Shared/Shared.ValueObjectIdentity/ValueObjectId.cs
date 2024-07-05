using System.Runtime.Serialization;


namespace Shared.ValueObjectIdentity;

public interface IValueObjectId : IComparable, ISpanFormattable, IUtf8SpanFormattable
{
	long Value { get; }
	bool HasValue { get; }
	string ToString();
}

public class ValueObjectId(long Value) : IValueObjectId, IComparable<ValueObjectId>, ISpanParsable<ValueObjectId>
{
	public ValueObjectId(string value) : this(Parse(value)) { }
	public ValueObjectId() : this(ValueObjectId.Empty) { }
	public ValueObjectId(ValueObjectId id) : this(id.Value) { }

	public static long Empty => new();

	public int CompareTo(ValueObjectId other) => Value.CompareTo(other.Value);

	[IgnoreDataMember]
	public bool HasValue => Value != ValueObjectId.Empty;

	public long Value => throw new NotImplementedException();

	public int CompareTo(object? obj) => Value.CompareTo(obj);
	public string ToString(string? format, IFormatProvider? formatProvider) => Value.ToString(format, formatProvider);

	public bool TryFormat(
		Span<char> destination,
		out int charsWritten,
		ReadOnlySpan<char> format,
		IFormatProvider? provider
	) => Value.TryFormat(destination, out charsWritten, format, provider);

	public bool TryFormat(
		Span<byte> utf8Destination,
		out int bytesWritten,
		ReadOnlySpan<char> format,
		IFormatProvider? provider
	) => Value.TryFormat(utf8Destination, out bytesWritten, format, provider);

	public override string ToString() => Value.ToString();
	public static ValueObjectId Parse(string s, IFormatProvider? provider) => Parse(s);

	public static bool TryParse(string? s, IFormatProvider? provider, out ValueObjectId result) => TryParse(s, out result);

	public static ValueObjectId Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		if (TryParse(s, provider, out var id))
			return id;

		throw new ArgumentException("Could not parse value into a valid long");
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out ValueObjectId id)
	{
		if (long.TryParse(s, out var result))
		{
			id = new ValueObjectId(result);
			return true;
		}

		id = new ValueObjectId();
		return false;
	}

	public static bool IsValid(string? s) => TryParse(s, out _);

	public static ValueObjectId Parse(string value)
	{
		if (long.TryParse(value, out var result))
			return new ValueObjectId(result);


		throw new ArgumentException("Could not parse value into a valid long");
	}

	public static ValueObjectId Parse(ReadOnlySpan<byte> base32) => new(long.Parse(base32));
	public static ValueObjectId From(ValueObjectId id) => new(id.Value);
	public static ValueObjectId NewId() => new(IdGenerator.NewId());

	public static string ToString(ValueObjectId id) => id.ToString();


	public static bool TryParse(string? value, out ValueObjectId id)
	{
		if (long.TryParse(value, out var result))
		{
			id = new ValueObjectId(result);
			return true;
		}

		id = new ValueObjectId();
		return false;
	}

	public static explicit operator ValueObjectId(string value) => new(value);
	public static implicit operator ValueObjectId(long value) => new(value);
	public static explicit operator string(ValueObjectId value) => value.ToString();
	public static explicit operator long(ValueObjectId value) => value.Value;
}

public class ValueObjectId<T>(long Value) : IValueObjectId, IComparable<ValueObjectId<T>>, ISpanParsable<ValueObjectId<T>>
{
	public ValueObjectId(string value) : this(Parse(value)) { }
	public ValueObjectId() : this(ValueObjectId<T>.Empty) { }
	public ValueObjectId(ValueObjectId id) : this(id.Value) { }
	public ValueObjectId(ValueObjectId<T> id) : this(id.Value) { }

	public static long Empty => new();

	public int CompareTo(ValueObjectId<T> other) => Value.CompareTo(other.Value);

	[IgnoreDataMember]
	public bool HasValue => Value != ValueObjectId<T>.Empty;

	public long Value => throw new NotImplementedException();

	public int CompareTo(object? obj) => Value.CompareTo(obj);
	public string ToString(string? format, IFormatProvider? formatProvider) => Value.ToString(format, formatProvider);

	public bool TryFormat(
		Span<char> destination,
		out int charsWritten,
		ReadOnlySpan<char> format,
		IFormatProvider? provider
	) => Value.TryFormat(destination, out charsWritten, format, provider);

	public bool TryFormat(
		Span<byte> utf8Destination,
		out int bytesWritten,
		ReadOnlySpan<char> format,
		IFormatProvider? provider
	) => Value.TryFormat(utf8Destination, out bytesWritten, format, provider);

	public override string ToString() => Value.ToString();
	public static ValueObjectId<T> Parse(string s, IFormatProvider? provider) => Parse(s);

	public static bool TryParse(string? s, IFormatProvider? provider, out ValueObjectId<T> result) => TryParse(s, out result);

	public static ValueObjectId<T> Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		if (TryParse(s, provider, out var id))
			return id;

		throw new ArgumentException("Could not parse value into a valid long");
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out ValueObjectId<T> id)
	{
		if (long.TryParse(s, out var result))
		{
			id = new ValueObjectId<T>(result);
			return true;
		}

		id = new ValueObjectId<T>();
		return false;
	}

	public static bool IsValid(string? s) => TryParse(s, out _);

	public static ValueObjectId<T> Parse(string value)
	{
		if (long.TryParse(value, out var result))
			return new ValueObjectId<T>(result);


		throw new ArgumentException("Could not parse value into a valid long");
	}

	public static ValueObjectId<T> Parse(ReadOnlySpan<byte> base32) => new(long.Parse(base32));
	public static ValueObjectId<T> From(ValueObjectId<T> id) => new(id.Value);
	public static ValueObjectId<T> NewId() => new(IdGenerator.NewId());

	public static string ToString(ValueObjectId id) => id.ToString();


	public static bool TryParse(string? value, out ValueObjectId<T> id)
	{
		if (long.TryParse(value, out var result))
		{
			id = new ValueObjectId<T>(result);
			return true;
		}

		id = new ValueObjectId<T>();
		return false;
	}

	public static explicit operator ValueObjectId<T>(string value) => new(value);
	public static implicit operator ValueObjectId<T>(long value) => new(value);
	public static explicit operator string(ValueObjectId<T> value) => value.ToString();
	public static explicit operator long(ValueObjectId<T> value) => value.Value;
}