namespace Infrastructure.ValueTypes;

/// <remarks>
/// <see href="https://ai.google.dev/api/files#File">Gemini API Reference</see>
/// </remarks>
public record GeminiFile(string Name, string Uri);