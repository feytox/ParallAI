using ParallAI.Core.States.Common;
using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.States;

public class CompareSettingsState : SettingsState
{
    public List<CompareElement> ConfiguredElements { get; private set; } = [];

    public void AddElement(CompareElement element) => ConfiguredElements.Add(element);

    public void RemoveAt(int index) => ConfiguredElements.RemoveAt(index);
}

public static class CompareSettingsStateExtensions
{
    public static OrchestratorSettingsState ToOrchestratorState(this CompareSettingsState state)
    {
        var elements = state.ConfiguredElements.ToArray();
        return new OrchestratorSettingsState(elements);
    }
}