namespace ParallAI.Core.ValueTypes;

public abstract record AiMessage(Role Role = Role.User);

public record TextMessage(string Text, Role Role = Role.User) : AiMessage(Role);

public record FileMessage(string Text, AiFileInfo[] Files, Role Role) : TextMessage(Text, Role)
{
    private const string DefaultText = "Describe content";

    public static FileMessage Create(AiFileInfo[] files, string? text, Role role = Role.User)
    {
        return new FileMessage(text ?? DefaultText, files, role);
    }
}

public enum Role
{
    User,
    Assistant,
}