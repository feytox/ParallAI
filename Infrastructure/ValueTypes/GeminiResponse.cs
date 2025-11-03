using AICore.ValueTypes;

namespace Infrastructure.ValueTypes;

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