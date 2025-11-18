using AICore.Entities;
using AICore.States;
using AICore.ValueTypes;
using TeleBotInfr.StateActions;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;

namespace TeleBot.Commands.Presets.StateActions;

public class PresetThinkingBudgetAction : IStepStateAction<PresetState, PresetStep>
{
    public PresetStep StateStep => PresetStep.ThinkingBudget;
    
    public async Task<bool> Execute(PresetState state, Message message, ITelegramBotClient bot, User user)
    {
        if (!int.TryParse(message.Text, out var thinkingBudget))
        {
            await bot.SendMessage(message.Chat, "Бюджет размышлений должен быть целым числом. Попробуй ещё раз");
            return false;
        }

        state.ThinkingBudget = thinkingBudget;

        var preset = new Preset(
            Guid.NewGuid(),
            state.Name!,
            new PromptSettings(state.SystemPrompt, state.Temperature, state.ThinkingBudget));
        user.AddPreset(preset);
        await bot.SendMessage(message.Chat, $"Вы добавили новый пресет {state.Name}");
        return true;
    }
}