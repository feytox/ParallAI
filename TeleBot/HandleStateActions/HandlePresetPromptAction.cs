#region

using AICore.States;
using User = AICore.Entities.User;
using Telegram.Bot;
using Telegram.Bot.Types;

#endregion

namespace TeleBot.HandleStateActions;

public class HandlePresetPromptAction : IStateAction
{
    public UserStateType HandledState => UserStateType.PresetWaitPrompt;

    public async Task Execute(Message message, User user, ITelegramBotClient bot)
    {
        if (string.IsNullOrWhiteSpace(message.Text))
        {
            await bot.SendMessage(message.Chat, "Промпт не может быть пустым.");
            return;
        }

        var state = user.StateMachine.TryGetState<PresetState>();
        state.Prompt = message.Text;
        user.StateMachine.MoveNext();
        
        await bot.SendMessage(message.Chat, "ПЕРВЫЙ СКИЛ И ТРЕТИЙ БЛЯЯЯТЬ! Укажи температуру.");
    }
}