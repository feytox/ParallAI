using AICore.Services;
using AICore.ValueTypes;
using TeleBot.Example.States;
using TeleBot.Services;
using TeleBot.Util;
using TeleBotInfr.StateActions;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;

namespace TeleBot.Example.StateActions;

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

        var response = await genService.Generate(model, prompt, PromptSettings.Default);
        await bot.SendMessage(message.Chat, response.Text);
        return true;
    }
}