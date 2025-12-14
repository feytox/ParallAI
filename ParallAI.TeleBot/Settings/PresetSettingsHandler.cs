using System.Globalization;
using ParallAI.Core.States;
using ParallAI.Core.ValueTypes;
using ParallAI.TeleBot.Core.Settings;
using ParallAI.TeleBot.Core.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Settings;

public class PresetSettingsHandler() : StandardSettingsHandler<PresetSettingsState>(CallbackTag, CreateParts)
{
    public const string CallbackTag = "preset_settings";
    public const string CancelPartTag = "cancelPart_preset";

    protected override string GetPartsMessage(PresetSettingsState state)
    {
        return $"Настройки пресета:\n\n📝 Название: {state.Name.ToDisplay(maxLength: 300)}\n"
               + state.ToFormattedString();
    }

    protected override async Task<bool> SaveSettingsToUser(PresetSettingsState state, CallbackQuery query,
        ITelegramBotClient bot, User user)
    {
        if (state.PresetId is null)
            SaveNewPreset(state, user);
        else
            ApplyChanges(state, user);
        await bot.EditCallbackMessage(query, "Пресет сохранён");
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
            .AddSimple("Название", CancelPartTag,"Введите название пресета", "",
                text => text,
                state => state.Name)
            .AddSimple("Системный промпт", CancelPartTag,"Введите системный промпт", "",
                text => text,
                state => state.SystemPrompt)
            .AddSimple("Температура", CancelPartTag,"Введите температуру (число от 0 до 2)",
                "Ошибка: Ожидается числом от 0 до 2. Попробуйте ещё раз",
                ParseDecimal, 
                state => state.Temperature)
            .AddEnum<ThinkingBudget>(CallbackTag, CancelPartTag,"Размышления",
                "Выберите бюджет размышлений",
                value => value != ThinkingBudget.Unknown,
                (state, budget) => state.ThinkingBudget = budget);
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