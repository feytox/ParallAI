using ParallAI.Core.Repositories;
using ParallAI.Core.States;
using ParallAI.Core.ValueTypes;
using ParallAI.TeleBot.Callback.CallbackArgs;
using ParallAI.TeleBot.Commands;
using ParallAI.TeleBot.Core.Callback;
using ParallAI.TeleBot.Core.Callback.Common;
using ParallAI.TeleBot.Core.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;


namespace ParallAI.TeleBot.Callback;

[CallbackQuery(Tag)]
public class AskCallback(IRepository<User, long> users) : UserCallbackQuery<AskArgs>(users)
{
    private const string Tag = "ask";
    
    protected override async Task Handle(CallbackQuery query, CallbackData<AskArgs> data, ITelegramBotClient bot, User user)
    {
        switch (data.Args.RequestMode)
        {
            case "single":
                await ChooseRequestConfig(query, bot, user, RequestMode.Single);
                break;
            case "continuous":
                await ChooseRequestConfig(query, bot, user, RequestMode.Continuous);
                break;
            case "settings":
                CreateRequestConfig(user);
                break;
        }
    }
    
    public static InlineKeyboardButton Create(string text, string content)
    {
        return InlineKeyboardButton.WithCallbackData(text, $"{Tag}:{content}");
    }

    private static async Task ChooseRequestConfig(CallbackQuery query, ITelegramBotClient bot, 
        User user, RequestMode requestMode)
    {
        if (user.ChosenModel is null)
        {
            await HandleNotSetModel(query, bot);
            return;
        }

        var model = user.GetModel(user.ChosenModel.Id);
        var preset = user.ChosenPreset is not null ? user.GetPreset(user.ChosenPreset.Id) : null;
        var requestConfig = new RequestConfig(model!, preset, requestMode);
        var state = new RequestState(requestConfig);
        user.StateMachine.Push(state);
    }

    private static async Task HandleNotSetModel(CallbackQuery query, ITelegramBotClient bot)
    {
        var markup = new InlineKeyboardMarkup(CommandCallback.Create<ModelsCommand>("Выбрать модель"));
        await bot.EditCallbackMessage(query, 
            "Чтобы отправлять одиночные запросы, выберите модель", 
            replyMarkup: markup);
    }
    
    private static void CreateRequestConfig(User user)
    {
        var state = new RequestSettingsState();
        user.StateMachine.Push(state);
    }
}