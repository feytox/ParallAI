#region

using AICore.States;
using User = AICore.Entities.User;
using Telegram.Bot;
using Telegram.Bot.Types;

#endregion

namespace TeleBot.HandleStateActions;

public class HandlePresetNameAction : StateAction<PresetState>
{
    public override bool CanHandle(UserState? state)
    {
        if (state is not PresetState ps) return false;
        return ps.Current == PresetStep.Name;
    }

    public override async Task Execute(Message message, ITelegramBotClient bot, User user)
    {
        var presetState = GetState(user.StateMachine.Current!);
        presetState.Name = message.Text;
        user.StateMachine.NextStepOrNothing();

        await bot.SendMessage(message.Chat, "Ямете кудасай! Теперь выбери модель.");
    }
}