using TeleBot.Example.States;
using TeleBot.StateActions.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;

namespace TeleBot.Example.StateActions;

public class PresetPromptAction : IStepStateAction<PresetState, PresetStep>
{
    public PresetStep StateStep => PresetStep.Prompt;
    
    public async Task<bool> Execute(PresetState state, Message message, ITelegramBotClient bot, User user)
    {
        if (string.IsNullOrWhiteSpace(message.Text))
        {
            await bot.SendMessage(message.Chat, "Промпт не может быть пустым.");
            return false;
        }
        
        state.Prompt = message.Text;
        
        await bot.SendMessage(message.Chat, "ПЕРВЫЙ СКИЛ И ТРЕТИЙ БЛЯЯЯТЬ! Укажи температуру.");
        return true;
    }
}