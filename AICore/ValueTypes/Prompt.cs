namespace AICore.ValueTypes;

public abstract record Prompt;

public record TextPrompt(string Text) : Prompt;

public record FilePrompt(string Text, string[] FileIds) : Prompt;