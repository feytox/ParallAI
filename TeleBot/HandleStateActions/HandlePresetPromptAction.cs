#region

using AICore.States;
using User = AICore.Entities.User;
using Telegram.Bot;
using Telegram.Bot.Types;

#endregion

namespace TeleBot.HandleStateActions;

public class HandlePresetPromptAction : StateAction<PresetState>
{
    public override bool CanHandle(UserState? state)
    {
        if (state is not PresetState ps) return false;
        return ps.Current == PresetStep.Prompt;
    }

    public override async Task Execute(Message message, ITelegramBotClient bot, User user)
    {
        if (string.IsNullOrWhiteSpace(message.Text))
        {
            await bot.SendMessage(message.Chat, "Промпт не может быть пустым.");
            return;
        }

        var presetState = GetState(user.StateMachine.Current!);
        presetState.Prompt = message.Text;
        user.StateMachine.NextStepOrNothing();
        
        await bot.SendMessage(message.Chat, "ПЕРВЫЙ СКИЛ И ТРЕТИЙ БЛЯЯЯТЬ! Укажи температуру.");
    }
}