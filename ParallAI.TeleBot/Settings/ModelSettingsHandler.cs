using ParallAI.Core.States;
using ParallAI.TeleBot.Core.Settings;
using ParallAI.TeleBot.Core.Util;
using ParallAI.TeleBot.Settings.Provider;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Settings;

public class ModelSettingsHandler() : StandardSettingsHandler<ModelSettingsState>(CallbackTag, CreateParts)
{
    public const string CallbackTag = "model_settings";

    protected override async Task<bool> SaveSettingsToUser(ModelSettingsState state, CallbackQuery query,
        ITelegramBotClient bot, User user)
    {
        if (state.Id is null)
            SaveNewModel(state, user);
        else
            ApplyChanges(state, user);
        await bot.EditCallbackMessage(query, "Модель сохранена");
        return true;
    }

    protected override string GetPartsMessage(ModelSettingsState state)
    {
        return $"Настройки модели:\n\n🏷️ Название: {state.DisplayName.ToDisplay(maxLength: 300)}"
               + state.ToFormattedString();
    }

    private static void SaveNewModel(ModelSettingsState state, User user)
    {
        var model = state.ToModel();
        user.AddModel(model);
    }

    private static void ApplyChanges(ModelSettingsState state, User user)
    {
        var savedModel = user.GetModel(state.Id!.Value);
        if (savedModel is null)
            SaveNewModel(state, user);
        else
            state.ApplyChanges(savedModel);
    }

    private static void CreateParts(SettingsPartsBuilder<ModelSettingsState> builder)
    {
        builder
            .AddSimple("Название", "Введите название модели", "",
                text => text,
                state => state.DisplayName)
            .AddSimple("ID модели", "Введите ID модели", "",
                text => text,
                state => state.ModelId)
            .Add(new ProviderSettingsPart("Провайдер"));
    }
}