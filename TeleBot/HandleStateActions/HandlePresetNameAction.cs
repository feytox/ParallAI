#region

using AICore.States;
using User = AICore.Entities.User;
using Telegram.Bot;
using Telegram.Bot.Types;

#endregion

namespace TeleBot.HandleStateActions;

public class HandlePresetNameAction : IStateAction
{
    public UserStateType HandledState => UserStateType.PresetWaitName;

    public async Task Execute(Message message, User user, ITelegramBotClient bot)
    {
        var state = user.StateMachine.TryGetState<PresetState>(); // я не знаю как исправить, просто спрятал тупой даун каст
        state.Name = message.Text;
        user.StateMachine.MoveNext();
        
        await bot.SendMessage(message.Chat, "Ямете кудасай! Теперь выбери модель.");
    }
}