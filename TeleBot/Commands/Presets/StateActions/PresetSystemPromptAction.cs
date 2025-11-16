using TeleBot.Example.States;
using TeleBot.StateActions.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;

namespace TeleBot.Example.StateActions;

public class PresetSystemPromptAction : IStepStateAction<PresetState, PresetStep>
{
    public PresetStep StateStep => PresetStep.SystemPrompt;
    
    public async Task<bool> Execute(PresetState state, Message message, ITelegramBotClient bot, User user)
    {
        if (string.IsNullOrWhiteSpace(message.Text))
            message.Text = "";
        state.SystemPrompt = message.Text;
        await bot.SendMessage(message.Chat, "Укажи температуру (любое число от 0 до 2)");
        return true;
    }
}