using ParallAI.Core.Repositories;
using ParallAI.TeleBot.Core.Callback;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Callback;

[CallbackQuery(Tag)]
public class CancelCallback(IRepository<User, long> users) : UserCallbackQuery(users)
{
    private const string Tag = "cancel";
    
    protected override async Task Handle(CallbackQuery callbackQuery, ITelegramBotClient bot, User user)
    {
        if (callbackQuery.Message is not null)
        {
            await bot.DeleteMessage(callbackQuery.From.Id, callbackQuery.Message!.Id);
        }
        
        try
        {
            user.StateMachine.Pop();
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Unable to pop the default UserState."))
        {
            await bot.SendMessage(callbackQuery.From.Id, "Сейчас нет команды, которую можно отменить");
            return;
        }

        await bot.SendMessage(callbackQuery.From.Id, "Команда отменена");
    }
}