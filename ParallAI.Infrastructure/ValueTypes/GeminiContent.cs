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

    public static GeminiContent CreateFromText(string text, MessageRole role)
    {
        return new GeminiContent([new Part(Text: text)], role);
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

public static class GeminiContentMappings
{
    private static GeminiContent.MessageRole ToGeminiRole(this Role role)
    {
        return role switch
        {
            Role.User => GeminiContent.MessageRole.User,
            Role.Assistant => GeminiContent.MessageRole.Model,
            _ => throw new ArgumentException($"Unknown role: {role}")
        };
    }

    public static GeminiContent ToGeminiContent(this TextMessage message) =>
        GeminiContent.CreateFromText(message.Text, message.Role.ToGeminiRole());

    public static GeminiContent ToGeminiContent(this FileMessage message, IEnumerable<string> fileUrls)
    {
        var parts = fileUrls
            .Select(url => new GeminiContent.Part(FileData: new GeminiContent.FileData(url)))
            .Append(new GeminiContent.Part(message.Text))
            .Reverse()
            .ToArray();

        return new GeminiContent(parts, message.Role.ToGeminiRole());
    }
}