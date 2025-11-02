#region

using AICore.States;
using User = AICore.Entities.User;
using Telegram.Bot;
using Telegram.Bot.Types;

#endregion

namespace TeleBot.HandleStateActions;

public class HandlePresetTemperatureAction
    : IStateAction
{
    public UserStateType HandledState => UserStateType.PresetWaitTemperature;

    public async Task Execute(Message message, User user, ITelegramBotClient bot)
    {
        if (!float.TryParse(message.Text?.Replace(',', '.'), out var temperature) || temperature < 0 || temperature > 2)
        {
            await bot.SendMessage(message.Chat, "Это не похоже на правильное число. Температура должна быть от 0 до 2. Попробуй ещё раз.");
            return;
        }

        var state = user.StateMachine.TryGetState<PresetState>();
        state.Temperature = temperature;
        //хз что дальше делать с собранными данными
        user.StateMachine.MoveNext();
        
        await bot.SendMessage(message.Chat, $"Бака-братик, теперь у нас есть новый пресет!");
    }
}