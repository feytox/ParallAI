using System.Globalization;
using ParallAI.Core.Entities;
using ParallAI.Core.States;
using ParallAI.Core.ValueTypes;
using ParallAI.TeleBot.Core.StateActions;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.StateActions.OldPresets;

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
        
        var preset = new Preset(
            Guid.NewGuid(),
            state.Name!,
            new PromptSettings(state.SystemPrompt, state.Temperature));
        user.AddPreset(preset);
        await bot.SendMessage(message.Chat, $"Вы добавили новый пресет {state.Name}");
        return true;
    }
}