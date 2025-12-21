using ParallAI.Core.Entities;
using ParallAI.Core.States.Common;
using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.States;

public class ModelSettingsState(Guid? id) : SettingsState
{
    public Guid? Id { get; private set; } = id;
    public string? ModelId { get; set; }
    public string? DisplayName { get; set; }
    public AiProvider? Provider { get; set; }
}

public static class ModelSettingsStateExtensions
{
    public static AiModel ToModel(this ModelSettingsState state)
    {
        var displayName = state.DisplayName ?? state.ModelId;
        return new AiModel(Guid.NewGuid(), state.ModelId!, displayName!, state.Provider!);
    }

    public static ModelSettingsState ToState(this AiModel model)
    {
        return new ModelSettingsState(model.Id)
        {
            ModelId = model.ModelId,
            DisplayName = model.DisplayName,
            Provider = model.Provider
        };
    }

    public static void ApplyChanges(this ModelSettingsState state, AiModel model)
    {
        model.ModelId = state.ModelId!;
        model.DisplayName = state.DisplayName!;
        model.Provider = state.Provider!;
    }
}