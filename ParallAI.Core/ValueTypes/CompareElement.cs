using ParallAI.Core.Entities;

namespace ParallAI.Core.ValueTypes;

public record CompareElement(Preset Preset, AiModel Model);

internal record CompareElementDto(Guid PresetId, Guid ModelId);

internal static class CompareElementExtensions
{
    public static CompareElementDto MapToDto(this CompareElement element)
    {
        return new CompareElementDto(element.Preset.Id, element.Model.Id);
    }
}