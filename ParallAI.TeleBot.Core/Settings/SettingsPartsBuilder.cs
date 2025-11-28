using ParallAI.Core.States.Common;

namespace ParallAI.TeleBot.Core.Settings;

public class SettingsPartsBuilder<TState> where TState : SettingsState
{
    private readonly List<SettingsPart<TState>> parts = [];

    public SettingsPartsBuilder<TState> AddSimple<TValue>(string name, string inputMessage, string failMessage,
        Func<string, TValue?> parser, Action<TState, TValue> saver)
    {
        var part = new SimpleSettingsPart<TValue, TState>(name, inputMessage, failMessage, parser, saver);
        parts.Add(part);
        return this;
    }

    public SettingsPartsBuilder<TState> Add(SettingsPart<TState> part)
    {
        parts.Add(part);
        return this;
    }

    public SettingsPart<TState>[] Build() => parts.ToArray();
}