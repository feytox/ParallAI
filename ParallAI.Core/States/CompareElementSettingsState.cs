using ParallAI.Core.Entities;
using ParallAI.Core.States.Common;
using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.States;

public class CompareElementSettingsState : SettingsState
{
    public Preset? Preset { get; set; }
    public AiModel? Model { get; set; }
}

public static class CompareElementSettingsStateExtensions
{
    public static IEnumerable<string> ToTextLines(this CompareElementSettingsState state)
    {
        if (state.Model is not null)
            yield return $"Модель: {state.Model.DisplayName}";
        
        if (state.Preset is not null)
            yield return $"Пресет: {state.Preset.Name}";
    }

    public static CompareElement ToElement(this CompareElementSettingsState state)
    {
        return new CompareElement(state.Preset!, state.Model!);
    }
}