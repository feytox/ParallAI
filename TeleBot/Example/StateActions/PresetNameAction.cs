using TeleBot.Example.States;
using TeleBot.StateActions.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;

namespace TeleBot.Example.StateActions;

public class PresetNameAction : IStepStateAction<PresetState, PresetStep>
{
    public PresetStep StateStep => PresetStep.Name;
    
    public async Task<bool> Execute(PresetState state, Message message, ITelegramBotClient bot, User user)
    {
        state.Name = message.Text;
        await bot.SendMessage(message.Chat, "Ямете кудасай! Теперь выбери модель.");
        return true;
    }
}