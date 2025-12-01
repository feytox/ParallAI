using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Core.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Settings;

public class ModelSettingsHandler() : SettingsHandler<ModelSettingsState>(Tag, CreateParts)
{
    public const string Tag = "model_settings";
    
    private static readonly string AiProviderType;

    protected override async Task SaveSettingsToUser(ModelSettingsState state, ChatId chatId, ITelegramBotClient bot, User user)
    {
        if (state.Id is null)
            SaveNewModel(state, user);
        else
            ApplyChanges(state, user);
        await bot.SendMessage(chatId, "Модель сохранена");
    }

    protected override string GetPartsMessage(ModelSettingsState state)
    {
        return "Настройки моделей"; // TODO: добавить отображение текущих настроек
    }

    private static void SaveNewModel(ModelSettingsState state, User user)
    {
        var model = state.ToModel();
        user.AddModel(model);
    }

    private static void ApplyChanges(ModelSettingsState state, User user)
    {
        var savedModel = user.UserModels.FirstOrDefault(model => model.Id == state.Id);
        if (savedModel is null)
            SaveNewModel(state, user);
        else
            state.ApplyChanges(savedModel);
    }

    private static void CreateParts(SettingsPartsBuilder<ModelSettingsState> builder)
    {
        builder
            .AddSimple("Название", "Введите название модели", "",
                text => text, (state, value) => state.DisplayName = value)
            .AddSimple("ID модели", "Введите ID модели", "",
                text => text, (state, value) => state.ModelId = value)
            .Add(new ProviderSettingsPart("Провайдер"));
    }
}