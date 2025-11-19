using ParallAI.Core.ValueTypes;

namespace ParallAI.Infrastructure.ValueTypes;

/// <remarks>
/// <see href="https://ai.google.dev/api/generate-content#v1beta.GenerateContentResponse">Gemini API Reference</see>
/// </remarks>
public record GeminiResponse(GeminiResponse.Candidate[] Candidates) : IGenResponse
{
    public record Candidate(GeminiContent Content);

    public AiResponse ToTextResponse()
    {
        var candidate = Candidates.SingleOrDefault();
        if (candidate is null)
            throw new ArgumentException(
                $"Response should contain exactly 1 response candidate. Actual: {Candidates.Length}");

        var text = candidate.Content.GetTextResponse();
        return new AiResponse(text);
    }
}