using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;
using ParallAI.Core.States;
using ParallAI.TeleBot.Core.Callback.Common;
using ParallAI.TeleBot.Core.Settings;
using ParallAI.TeleBot.Core.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Callback;

[CallbackQuery(Tag)]
public class ModelCallback(IRepository<User, long> users) : SettingsElementCallback(users)
{
    private const string Tag = "model";
    
    protected override string GetElementInfo(int index, User user)
    {
        var model = GetModel(user, index);
        
        return $"Модель: {model.DisplayName.ToDisplay(maxLength: 300)}\n"
            + model.ToFormattedString();
    }

    protected override async Task HandleChoose(CallbackQuery query, int index, ITelegramBotClient bot, User user)
    {
        var model = GetModel(user, index);
        user.ChooseModel(model);
        
        await bot.EditCallbackMessage(query, $"Модель {model.DisplayName} выбрана");
    }

    protected override Task HandleEdit(CallbackQuery query, int index, ITelegramBotClient bot, User user)
    {
        var state = index == -1 ? new ModelSettingsState(null) : GetModel(user, index).ToState();
        user.StateMachine.Push(state);
        
        return Task.CompletedTask;
    }

    protected override async Task HandleRemove(CallbackQuery query, int index, ITelegramBotClient bot, User user)
    {
        var model = GetModel(user, index);
        user.DeleteModel(model);
        
        await bot.EditCallbackMessage(query, $"Модель {model.DisplayName} удалена");
    }

    protected override bool ContainsAt<T>(int index, User user) => GetModel(user, index) is T;

    public static InlineKeyboardButton Create(string text, string data)
    {
        return InlineKeyboardButton.WithCallbackData(text, $"{Tag}:{data}");
    }
    
    private static AiModel GetModel(User user, int index)
    {
        return user.UserModels[index];
    }
}