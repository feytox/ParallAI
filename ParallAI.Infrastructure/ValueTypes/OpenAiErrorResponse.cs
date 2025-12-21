namespace ParallAI.Infrastructure.ValueTypes;

public record OpenAiErrorResponse(OpenAiErrorResponse.OpenAiError Error)
{
    public record OpenAiError(string Message, string? Type, string? Param, string Code);
};