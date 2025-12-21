namespace ParallAI.Infrastructure.ValueTypes;

public record GeminiErrorResponse(GeminiErrorResponse.GeminiError Error)
{
    public record GeminiError(int Code, string Message, string Status);
}