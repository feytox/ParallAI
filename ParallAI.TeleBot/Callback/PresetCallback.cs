using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;
using ParallAI.Core.States;
using ParallAI.TeleBot.Core.Callback.Common;
using ParallAI.TeleBot.Core.Settings;
using ParallAI.TeleBot.Core.Util;
using ParallAI.TeleBot.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Callback;

[CallbackQuery(Tag)]
public class PresetCallback(IRepository<User, long> users, PresetSettingsHandler handler)
    : SettingsElementCallback(users)
{
    public const string Tag = "preset";

    protected override string GetElementInfo(int index, User user)
    {
        var preset = GetPreset(user, index);
        
        return $"Пресет: {preset.Name.ToDisplay(maxLength: 300)}\n\n"
               + preset.ToFormattedString();
    }

    protected override async Task HandleChoose(CallbackQuery query, int index, ITelegramBotClient bot, User user)
    {
        var preset = GetPreset(user, index);
        user.ChoosePreset(preset);

        await bot.EditCallbackMessage(query, $"Пресет {preset.Name} выбран");
    }

    protected override Task HandleEdit(CallbackQuery query, int index, ITelegramBotClient bot, User user)
    {
        var state = index == -1 ? new PresetSettingsState(null) : GetPreset(user, index).ToState();
        user.StateMachine.Push(state);
        
        return Task.CompletedTask;
    }

    protected override async Task HandleRemove(CallbackQuery query, int index,
        ITelegramBotClient bot, User user)
    {
        var preset = GetPreset(user, index);
        user.DeletePreset(preset);

        await bot.EditCallbackMessage(query, $"Пресет {preset.Name} удалён");
    }

    private static Preset GetPreset(User user, int index)
    {
        return user.UserPresets[index];
    }
    
    protected override object GetElement(int index, User user) => GetPreset(user, index);
}