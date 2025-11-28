using System.Globalization;
using ParallAI.Core.States;
using ParallAI.Core.ValueTypes;
using ParallAI.TeleBot.Core.Settings;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Settings;

public class PresetSettingsHandler() : SettingsHandler<PresetSettingsState>("preset_settings", CreateParts)
{
    private static readonly string ThinkingBudgets;

    protected override string GetPartsMessage(PresetSettingsState state)
    {
        return "Настройки пресетов"; // TODO: добавить отображение текущих настроек
    }

    protected override void SaveResult(PresetSettingsState state, User user)
    {
        if (state.PresetId is null)
            SaveNewPreset(state, user);
        else
            ApplyChanges(state, user);
    }

    private static void SaveNewPreset(PresetSettingsState state, User user)
    {
        var preset = state.ToPreset();
        user.AddPreset(preset);
    }

    private static void ApplyChanges(PresetSettingsState state, User user)
    {
        var savedPreset = user.UserPresets.FirstOrDefault(preset => preset.Id == state.PresetId);
        if (savedPreset is null)
            SaveNewPreset(state, user);
        else
            state.ApplyChanges(savedPreset);
    }

    private static void CreateParts(SettingsPartsBuilder<PresetSettingsState> builder)
    {
        builder
            .AddSimple("Название", "Введите название пресета", "",
                text => text, (state, value) => state.Name = value)
            .AddSimple("Системный промпт", "Введите системный промпт", "",
                text => text, (state, value) => state.SystemPrompt = value)
            .AddSimple("Температура", "Введите температуру (число от 0 до 2)",
                "Ошибка: Ожидается числом от 0 до 2. Попробуйте ещё раз",
                ParseDecimal, (state, value) => state.Temperature = value)
            .AddSimple("Размышления", $"Выберите бюджет размышлений:\n{ThinkingBudgets}",
                "Неправильный вариант. Попробуйте ещё раз",
                text => Enum.Parse<ThinkingBudget>(text, true), (state, budget) => state.ThinkingBudget = budget);
    }

    private static decimal? ParseDecimal(string text)
    {
        text = text.Replace(',', '.');
        if (decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out var temperature)
            && temperature is >= 0 and <= 2)
        {
            return temperature;
        }

        return null;
    }

    static PresetSettingsHandler()
    {
        var lines = Enum.GetNames<ThinkingBudget>().Select(name => $"- {name}");
        ThinkingBudgets = string.Join('\n', lines);
    }
}