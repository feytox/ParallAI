using ParallAI.Core.Services;
using ParallAI.Core.States;
using ParallAI.Core.ValueTypes;
using ParallAI.TeleBot.Core.StateActions;
using ParallAI.TeleBot.Services;
using ParallAI.TeleBot.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Example.StateActions;

public class RequestPromptAction(GenerationService genService, MediaGroupCollector groupCollector)
    : IStepStateAction<RequestState, RequestStep>
{
    public RequestStep StateStep => RequestStep.Prompt;

    public async Task<bool> Execute(RequestState state, Message message, ITelegramBotClient bot, User user)
    {
        var messages = await groupCollector.CollectMessages(message);
        if (messages is null)
            return false;
        
        var model = user.UserModels.First();
        var prompt = MessageExt.CreatePrompt(messages);
        var settings = PromptSettings.Default with { ThinkingBudget = ThinkingBudget.High };

        var response = await genService.Generate(model, [prompt], settings);
        await bot.SendMessage(message.Chat, response.Text);
        return true;
    }
}