using System.Text.Json.Serialization;
using AICore.ValueTypes;
using Infrastructure.Util;

namespace Infrastructure.ValueTypes;

public record GeminiContent(
    GeminiContent.Part[] Parts,
    [property: JsonConverter(typeof(JsonWebEnumConverter<GeminiContent.MessageRole>))]
    GeminiContent.MessageRole Role)
{
    public string GetTextResponse()
    {
        var part = Parts.SingleOrDefault();
        if (part is null)
            throw new ArgumentException($"Content should contain exactly 1 part. Actual: {Parts.Length}");

        return part.Text;
    }

    public static GeminiContent[] CreateFromPrompt(Prompt prompt) => [CreateFromText(prompt.Text)];

    public static GeminiContent CreateFromText(string text) => new([new Part(text)], MessageRole.User);

    public record Part(string Text);

    public enum MessageRole
    {
        User,
        Model
    }
}