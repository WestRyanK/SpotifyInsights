using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpotifyInsights
{
	internal class DateTimeJsonConverter : JsonConverter<DateTime>
	{
		public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			string dateString = reader.GetString() ?? string.Empty;
			return DateTime.Parse(dateString);
		}

		public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.ToString());
		}
	}
}