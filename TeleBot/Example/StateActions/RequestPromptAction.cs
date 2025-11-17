using AICore.Services;
using AICore.ValueTypes;
using TeleBot.Example.States;
using TeleBot.StateActions.Common;
using TeleBot.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;

namespace TeleBot.Example.StateActions;

public class RequestPromptAction(GenerationService genService) : IStepStateAction<RequestState, RequestStep>
{
    public RequestStep StateStep => RequestStep.Prompt;
    
    public async Task<bool> Execute(RequestState state, Message message, ITelegramBotClient bot, User user)
    {
        var model = user.UserModels.First(aiModel => aiModel.ModelId.Contains("sherlock"));
        var prompt = message.CreatePrompt();
        
        var response = await genService.Generate(model, prompt, PromptSettings.Default);
        await bot.SendMessage(message.Chat, response.Text);
        return true;
    }
}