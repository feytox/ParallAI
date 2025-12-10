namespace ParallAI.Infrastructure.ValueTypes;

public record OpenRouterErrorResponse(OpenRouterErrorResponse.OpenRouterError Error)
{
    public record OpenRouterError(string Message, int Code);
};