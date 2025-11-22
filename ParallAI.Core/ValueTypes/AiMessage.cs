namespace ParallAI.Core.ValueTypes;

public abstract record AiMessage(Role role = Role.User)
{
    public Role Role { get; init; } = role;
}

public record TextMessage(string Text, Role role = Role.User) : AiMessage(role);

public record FileMessage(string Text, AiFileInfo[] Files, Role role) : TextMessage(Text, role)
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