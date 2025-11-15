using Infrastructure.ValueTypes;

namespace AICore.ValueTypes;

public abstract record Prompt
{
    // TODO: а как прикрепляются несколько файлов к одному сообщению?
    public static Prompt Create(string text, AiFileInfo? file)
    {
        if (file is null)
            return new TextPrompt(text);

        return new FilePrompt(text, [file]);
    }
}

public record TextPrompt(string Text) : Prompt;

public record FilePrompt(string Text, AiFileInfo[] Files) : Prompt;