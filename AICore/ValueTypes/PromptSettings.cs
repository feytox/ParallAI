namespace AICore.ValueTypes;

// TODO: maybe add other settings
public record PromptSettings(string SystemInstructions, double Temperature)
{
    public static PromptSettings Default = new PromptSettings("", 0.7d);
}