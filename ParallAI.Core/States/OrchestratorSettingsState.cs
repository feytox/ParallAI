using ParallAI.Core.Entities;
using ParallAI.Core.States.Common;
using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.States;

public class OrchestratorSettingsState(CompareElement[] configuredElements) : SettingsState
{
    public CompareElement[] ConfiguredElements { get; private init; } = configuredElements;
    public Preset? Preset { get; set; }
    public AiModel? Model { get; set; }
}

public static class OrchestratorSettingsStateExtensions
{
    public static CompareConfig Build(this OrchestratorSettingsState state)
    {
        var creationTime = DateTime.UtcNow;
        var orchestrator = new CompareElement(state.Preset!, state.Model!);
        return new CompareConfig(state.ConfiguredElements, orchestrator, creationTime);
    }
}