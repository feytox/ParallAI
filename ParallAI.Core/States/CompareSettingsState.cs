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
    public static CompareConfig Build(this CompareSettingsState state)
    {
        var creationTime = DateTime.UtcNow;
        var elements = state.ConfiguredElements.SkipLast(1).ToArray();
        var orchestrator = state.ConfiguredElements.Last();
        
        return new CompareConfig(elements, orchestrator, creationTime);
    }
}