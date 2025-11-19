using ParallAI.Core.States;
using ParallAI.TeleBot.Core.StateActions;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Commands.Presets.StateActions;

public class PresetNameAction : IStepStateAction<PresetState, PresetStep>
{
    public PresetStep StateStep => PresetStep.Name;
    
    public async Task<bool> Execute(PresetState state, Message message, ITelegramBotClient bot, User user)
    {
        if (user.UserPresets.Any(p => string.Equals(p.Name, message.Text, StringComparison.CurrentCultureIgnoreCase)))
        {
            await bot.SendMessage(message.Chat, "Пресет с таким названием уже существует. Отправь другое название");
            return false;
        }
        state.Name = message.Text;
        await bot.SendMessage(message.Chat, "Напиши системный промпт");
        return true;
    }
}