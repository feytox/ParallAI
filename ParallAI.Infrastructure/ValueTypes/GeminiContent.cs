using System.Text.Json.Serialization;
using ParallAI.Core.ValueTypes;
using ParallAI.Infrastructure.Util;

namespace ParallAI.Infrastructure.ValueTypes;

/// <remarks>
/// <see href="https://ai.google.dev/api/caching#Content">Gemini API Reference</see>
/// </remarks>
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

        return part.Text!;
    }

    public static GeminiContent[] Create(TextPrompt prompt) => [CreateFromText(prompt.Text)];

    public static GeminiContent[] Create(FilePrompt prompt, IEnumerable<string> fileUrls)
    {
        var parts = fileUrls
            .Select(url => new Part(FileData: new FileData(url)))
            .Append(new Part(prompt.Text))
            .Reverse()
            .ToArray();

        return [new GeminiContent(parts, MessageRole.User)];
    }

    public static GeminiContent CreateFromText(string text)
    {
        return new GeminiContent([new Part(Text: text)], MessageRole.User);
    }

    public record Part(string? Text = null, FileData? FileData = null);

    public record FileData(string FileUri);

    public enum MessageRole
    {
        User,
        Model
    }
}