using Infrastructure.ValueTypes;

namespace AICore.ValueTypes;

public abstract record Prompt;

public record TextPrompt(string Text) : Prompt;

public record FilePrompt(string Text, AiFileInfo[] Files) : TextPrompt(Text)
{
    private const string DefaultText = "Describe content";

    public static FilePrompt Create(AiFileInfo[] files, string? text)
    {
        return new FilePrompt(text ?? DefaultText, files);
    }
}