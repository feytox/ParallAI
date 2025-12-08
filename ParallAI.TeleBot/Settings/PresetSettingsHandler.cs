using System.Globalization;
using ParallAI.Core.States;
using ParallAI.Core.ValueTypes;
using ParallAI.TeleBot.Core.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Settings;

public class PresetSettingsHandler() : StandardSettingsHandler<PresetSettingsState>(CallbackTag, CreateParts)
{
    public const string CallbackTag = "preset_settings";

    protected override string GetPartsMessage(PresetSettingsState state)
    {
        return "Настройки пресетов"; // TODO: добавить отображение текущих настроек
    }

    protected override async Task<bool> SaveSettingsToUser(PresetSettingsState state, ChatId chatId,
        ITelegramBotClient bot, User user)
    {
        if (state.PresetId is null)
            SaveNewPreset(state, user);
        else
            ApplyChanges(state, user);
        await bot.SendMessage(chatId, "Пресет сохранён");
        return true;
    }

    private static void SaveNewPreset(PresetSettingsState state, User user)
    {
        var preset = state.ToPreset();
        user.AddPreset(preset);
    }

    private static void ApplyChanges(PresetSettingsState state, User user)
    {
        var savedPreset = user.GetPreset(state.PresetId!.Value);
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
            .AddEnum<ThinkingBudget>(CallbackTag, "Размышления",
                "Выберите бюджет размышлений",
                (state, value) => state.ThinkingBudget = value,
                value => value != ThinkingBudget.Unknown);
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
}

// by дима комар (оригинальный файл утерян)
// ⠄⠄⠄⢀⡋⣡⣴⣶⣶⡀⠄⠄⠙⢿⣿⣿⣿⣿⣿⣴⣿⣿⣿⢃⣤⣄⣀⣥⣿⣿⠄
// ⠄⠄⢸⣇⠻⣿⣿⣿⣧⣀⢀⣠⡌⢻⣿⣿⣿⣿⣿⣿⣿⣿⣿⠿⠿⠿⣿⣿⣿⠄
// ⢀⢸⣿⣷⣤⣤⣤⣬⣙⣛⢿⣿⣿⣿⣿⣿⣿⡿⣿⣿⡍⠄⠄⢀⣤⣄⠉⠋⣰
// ⣼⣖⣿⣿⣿⣿⣿⣿⣿⣿⣿⢿⣿⣿⣿⣿⣿⢇⣿⣿⡷⠶⠶⢿⣿⣿⠇⢀⣤
// ⠘⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣽⣿⣿⣿⡇⣿⣿⣿⣿⣿⣿⣷⣶⣥⣴⣿⡗
// ⢀⠈⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡟⠄
// ⢸⣿⣦⣌⣛⣻⣿⣿⣧⠙⠛⠛⡭⠅⠒⠦⠭⣭⡻⣿⣿⣿⣿⣿⣿⣿⣿⡿⠃⠄
// ⠘⣿⣿⣿⣿⣿⣿⣿⣿⡆⠄⠄⠄⠄⠄⠄⠄⠄⠹⠈⢋⣽⣿⣿⣿⣿⣵⣾⠃⠄
// ⠄⠘⣿⣿⣿⣿⣿⣿⣿⣿⠄⣴⣿⣶⣄⠄⣴⣶⠄⢀⣾⣿⣿⣿⣿⣿⣿⠃⠄⠄
// ⠄⠄⠈⠻⣿⣿⣿⣿⣿⣿⡄⢻⣿⣿⣿⠄⣿⣿⡀⣾⣿⣿⣿⣿⣛⠛⠁⠄⠄⠄
// ⠄⠄⠄⠄⠈⠛⢿⣿⣿⣿⠁⠞⢿⣿⣿⡄⢿⣿⡇⣸⣿⣿⠿⠛⠁⠄⠄⠄⠄⠄
// ⠄⠄⠄⠄⠄⠄⠄⠉⠻⣿⣿⣾⣦⡙⠻⣷⣾⣿⠃⠿⠋⠁⠄⠄⠄⠄⠄⢀⣠⣴
// ⣿⣿⣿⣶⣶⣮⣥⣒⠲⢮⣝⡿⣿⣿⡆⣿⡿⠃⠄⠄⠄⠄⠄⠄⠄⣠⣴⣿⣿⣿