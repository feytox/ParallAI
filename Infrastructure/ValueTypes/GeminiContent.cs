using AICore.ValueTypes;

namespace Infrastructure.ValueTypes;

// TODO: use enum for roles
public record GeminiContent(GeminiContent.Part[] Parts, string? Role = null)
{
    public string GetTextResponse()
    {
        var part = Parts.SingleOrDefault();
        if (part is null)
            throw new ArgumentException($"Content should contain exactly 1 part. Actual: {Parts.Length}");

        return part.Text;
    }
    
    public static GeminiContent[] CreateFromPrompt(Prompt prompt) => [CreateFromText(prompt.Text)];

    public static GeminiContent CreateFromText(string text) => new([new Part(text)]);
    
    public record Part(string Text);
}