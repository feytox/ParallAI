namespace AICore.ValueTypes;

// TODO: add thinking budget
public record PromptSettings(string SystemInstructions, double Temperature)
{
    public static PromptSettings Default = new("", 0.7d);
}