using ParallAI.Core.Repositories;
using ParallAI.Core.States;
using ParallAI.TeleBot.Core.Callback.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Callback;

[CallbackQuery(CallbackTag)]
public class CompareCallback(IRepository<User, long> users) : UserCallbackQuery(users)
{
    private const string CallbackTag = "compare";

    protected override Task Handle(CallbackQuery query, ITelegramBotClient bot, User user)
    {
        var content = query.Data!.Split(':')[1];
        if (content == "+")
            CreateCompareConfig(user);
        else
            ChooseCompareConfig(user, int.Parse(content));
        
        return Task.CompletedTask;
    }

    private void CreateCompareConfig(User user)
    {
        var state = new CompareSettingsState();
        user.StateMachine.Push(state);
    }

    private void ChooseCompareConfig(User user, int configIndex)
    {
        var config = user.Comparisons[configIndex];
        var state = new CompareState(config);
        user.StateMachine.Push(state);
    }

    public static InlineKeyboardButton CreateButton(string text, string data) =>
        InlineKeyboardButton.WithCallbackData(text, $"{CallbackTag}:{data}");
}