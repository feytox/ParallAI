using System.Text.Json;
using System.Text.Json.Serialization;
using Infrastructure.ValueTypes;

namespace Infrastructure.Util;

public class OpenAiContentConverter : JsonConverter<OpenAiContent>
{
    public override OpenAiContent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.String:
                return OpenAiContent.Create(reader.GetString()!);
            case JsonTokenType.StartArray:
            {
                var items = JsonSerializer.Deserialize<ContentItem[]>(ref reader, options);
                return new OpenAiContent(items!);
            }
            default:
                throw new JsonException("Unsupported response's OpenAiContent");
        }
    }

    public override void Write(Utf8JsonWriter writer, OpenAiContent value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.Items, options);
    }
}