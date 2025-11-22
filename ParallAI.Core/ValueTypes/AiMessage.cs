namespace ParallAI.Core.ValueTypes;

public abstract record AiMessage;

public record TextMessage(string Text) : AiMessage;

public record FileMessage(string Text, AiFileInfo[] Files) : TextMessage(Text)
{
    private const string DefaultText = "Describe content";

    public static FileMessage Create(AiFileInfo[] files, string? text)
    {
        return new FileMessage(text ?? DefaultText, files);
    }
}