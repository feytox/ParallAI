using AICore.ValueTypes;

namespace Infrastructure.ValueTypes;

public record GeminiResponse(GeminiResponse.Candidate[] Candidates)
{
    public record Candidate(GeminiContent Content);
}

public static class GeminiResponseExt
{
    public static AiResponse ToTextResponse(this GeminiResponse response)
    {
        var candidate = response.Candidates.SingleOrDefault();
        if (candidate is null)
            throw new ArgumentException(
                $"Response should contain exactly 1 response candidate. Actual: {response.Candidates.Length}");

        var text = candidate.Content.GetTextResponse();
        return new AiResponse(text);
    }
}