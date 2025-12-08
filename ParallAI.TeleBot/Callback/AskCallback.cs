using ParallAI.Core.Repositories;
using ParallAI.Core.States;
using ParallAI.Core.ValueTypes;
using ParallAI.TeleBot.Core.Callback.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;


namespace ParallAI.TeleBot.Callback;

[CallbackQuery(CallbackTag)]
public class AskCallback(IRepository<User, long> users) : UserCallbackQuery(users)
{
    public const string CallbackTag = "ask";
    
    protected override async Task Handle(CallbackQuery callbackQuery, ITelegramBotClient bot, User user)
    {
        var content = callbackQuery.Data!.Split(':')[1];
        switch (content)
        {
            case "single":
                await ChooseRequestConfig(callbackQuery, bot, user, RequestMode.Single);
                break;
            case "continuous":
                await ChooseRequestConfig(callbackQuery, bot, user, RequestMode.Continuous);
                break;
            case "settings":
                await CreateRequestConfig(user);
                break;
        }
    }

    private async Task ChooseRequestConfig(
        CallbackQuery callbackQuery, ITelegramBotClient bot, User user, RequestMode requestMode)
    {
        if (user.ChosenModel is null)
        {
            await bot.SendMessage(callbackQuery.From.Id, "Чтобы отправлять запросы, необходимо выбрать модель");
            return;
        }

        var model = user.GetModel(user.ChosenModel.Id);
        var preset = user.ChosenPreset is not null ? user.GetPreset(user.ChosenPreset.Id) : null;
        var requestConfig = new RequestConfig(model!, preset, requestMode);
        var state = new RequestState(requestConfig);
        user.StateMachine.Push(state);
    }
    
    private async Task CreateRequestConfig(User user)
    {
        var state = new RequestSettingsState();
        user.StateMachine.Push(state);
    }
}