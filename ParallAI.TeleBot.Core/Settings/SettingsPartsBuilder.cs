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
    
    public SettingsPartsBuilder<TState> AddEnum<TEnum>(string tag, string name, string inputMessage, 
        Action<TState, TEnum> saver, Func<TEnum, bool> selector) where TEnum : struct, Enum
    {
        var part = new EnumSettingsPart<TEnum, TState>(tag, name, inputMessage, saver, selector);
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