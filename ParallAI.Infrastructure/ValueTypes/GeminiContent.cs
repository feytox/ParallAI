using System.Text.Json.Serialization;
using ParallAI.Core.ValueTypes;
using ParallAI.Infrastructure.Util;

namespace ParallAI.Infrastructure.ValueTypes;

/// <remarks>
/// <see href="https://ai.google.dev/api/caching#Content">Gemini API Reference</see>
/// </remarks>
public record GeminiContent(GeminiContent.Part[] Parts, GeminiContent.MessageRole Role)
{
    public string GetTextResponse()
    {
        var part = Parts.SingleOrDefault();
        if (part is null)
            throw new ArgumentException($"Content should contain exactly 1 part. Actual: {Parts.Length}");

        return part.Text!;
    }

    public static GeminiContent[] Create(TextMessage message) => [CreateFromText(message.Text)];

    public static GeminiContent[] Create(FileMessage message, IEnumerable<string> fileUrls)
    {
        var parts = fileUrls
            .Select(url => new Part(FileData: new FileData(url)))
            .Append(new Part(message.Text))
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
    
    [JsonConverter(typeof(JsonWebEnumConverter<MessageRole>))]
    public enum MessageRole
    {
        User,
        Model
    }
}