using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lookif.Library.Common.ValueConvertors;

public class DateOnlyJsonConverter : JsonConverter<DateOnly>

{
	public override void Write(Utf8JsonWriter writer, DateOnly date, JsonSerializerOptions options)
	{
		writer.WriteStringValue(date.ToString());
	}
	public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return DateOnly.FromDateTime(reader.GetDateTime());
	}
}
