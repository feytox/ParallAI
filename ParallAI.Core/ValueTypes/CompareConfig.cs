namespace ParallAI.Core.ValueTypes;

public record CompareConfig(CompareElement[] Elements, CompareElement Orchestrator, DateTime CreationTime);

internal record CompareConfigDto(CompareElementDto[] Elements, CompareElementDto Orchestrator, DateTime CreationTime);

internal static class CompareConfigExtensions
{
    public static CompareConfigDto MapToDto(this CompareConfig config)
    {
        var elements = config.Elements
            .Select(element => element.MapToDto())
            .ToArray();
        
        return new CompareConfigDto(elements, config.Orchestrator.MapToDto(), config.CreationTime);
    }
}