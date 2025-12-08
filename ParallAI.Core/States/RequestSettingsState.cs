using ParallAI.Core.Entities;
using ParallAI.Core.States.Common;

namespace ParallAI.Core.States;

public class RequestSettingsState : SettingsState
{
    public AiModel? Model { get; set; }
    public Preset? Preset { get; set; }
}

public static class RequestSettingsStateExtensions
{
    public static IEnumerable<string> ToTextLines(this RequestSettingsState state)
    {
        if (state.Model is not null)
            yield return $"Модель: {state.Model.DisplayName}";
        
        if (state.Preset is not null)
            yield return $"Пресет: {state.Preset.Name}";
    }
}