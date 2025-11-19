using System.Globalization;
using ParallAI.Core.States;
using ParallAI.TeleBot.Core.StateActions;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Commands.Presets.StateActions;

public class PresetTemperatureAction : IStepStateAction<PresetState, PresetStep>
{
    public PresetStep StateStep => PresetStep.Temperature;
    
    public async Task<bool> Execute(PresetState state, Message message, ITelegramBotClient bot, User user)
    {
        if (!decimal.TryParse(
                message.Text?.Replace(',', '.'),
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out var temperature)
            || temperature < 0
            || temperature > 2)
        {
            await bot.SendMessage(message.Chat,
                "Это не похоже на правильное число. Температура должна быть от 0 до 2. Попробуй ещё раз");
            return false;
        }
        
        state.Temperature = temperature;
        
        await bot.SendMessage(message.Chat, "Укажи бюджет размышлений");
        return true;
    }
}