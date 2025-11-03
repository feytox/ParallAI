#region

using AICore.States;
using User = AICore.Entities.User;
using Telegram.Bot;
using Telegram.Bot.Types;

#endregion

namespace TeleBot.HandleStateActions;

public class HandlePresetTemperatureAction : StateAction<PresetState>
{
    public override bool CanHandle(UserState? state)
    {
        if (state is not PresetState ps) return false;
        return ps.Current == PresetStep.Temperature;
    }

    public override async Task Execute(Message message, ITelegramBotClient bot, User user)
    {
        if (!float.TryParse(message.Text?.Replace(',', '.'), out var temperature) || temperature < 0 || temperature > 2)
        {
            await bot.SendMessage(message.Chat, "Это не похоже на правильное число. Температура должна быть от 0 до 2. Попробуй ещё раз.");
            return;
        }
        
        var presetState = GetState(user.StateMachine.Current!);
        presetState.Temperature = temperature;
        //хз что дальше делать с собранными данными
        user.StateMachine.NextStepOrNothing();
        
        await bot.SendMessage(message.Chat, $"Бака-братик, теперь у нас есть новый пресет!");
    }
}