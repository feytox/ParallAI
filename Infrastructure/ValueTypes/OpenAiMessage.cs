namespace Infrastructure.ValueTypes;

// TODO: use enum for roles
public record OpenAiMessage(string Content, string Role);