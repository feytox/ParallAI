using AICore;
using AICore.Exceptions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TeleBot.Util;
using TeleBotInfr.Callback;
using TeleBotInfr.Commands;
using TeleBotInfr.StateActions;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TeleBot;

public class Bot(
    IConfig config,
    ILogger<Bot> logger,
    CommandHandler commandHandler,
    StateHandler stateHandler,
    CallbackQueryHandler callbackQueryHandler) : IHostedService
{
    public ITelegramBotClient Client => client ?? throw new NullReferenceException("Bot is not initialized");

    private TelegramBotClient? client;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        client = new TelegramBotClient(config.BotToken, cancellationToken: cancellationToken);

        client.StartReceiving(HandleUpdate, HandleError, cancellationToken: cancellationToken);
        logger.LogInformation("Bot has been started.");
        return Task.CompletedTask;
    }

    private async Task HandleUpdate(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        try
        {
            await HandleUpdateOrThrow(bot, update);
        }
        catch (UserFriendlyException ex)
        {
            logger.LogError(ex.ToString());
            await TrySendMessage(bot, update, ex.UserMessage);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.ToString());
            await TrySendMessage(bot, update,
                $"Упс...произошла непредвиденная ошибка {ex.GetType().Name}. Все вопросы к @feytox");
        }
    }

    private async Task HandleUpdateOrThrow(ITelegramBotClient bot, Update update)
    {
        switch (update.Type)
        {
            case UpdateType.Message:
                await HandleMessage(bot, update.Message!);
                break;
            case UpdateType.CallbackQuery:
                await HandleCallbackQuery(bot, update.CallbackQuery!);
                break;
            default:
                throw new ArgumentException($"Unhandled update type: {update.Type}");
        }
    }

    private async Task HandleMessage(ITelegramBotClient bot, Message message)
    {
        var wasStateHandled = await stateHandler.HandleState(message, bot);
        if (!wasStateHandled)
            await commandHandler.HandleCommand(message, bot);
    }

    private async Task HandleCallbackQuery(ITelegramBotClient bot, CallbackQuery callbackQuery)
    {
        await callbackQueryHandler.HandleCallbackQuery(callbackQuery, bot);
    }

    private Task HandleError(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiExc => $"Telegram API Error: [{apiExc.ErrorCode}] {apiExc.Message}",
            _ => exception.ToString()
        };

        logger.LogError(errorMessage);
        return Task.CompletedTask;
    }

    private static async Task TrySendMessage(ITelegramBotClient bot, Update update, string message)
    {
        var chatId = update.GetChatId();
        if (chatId is not null)
            await bot.SendMessage(chatId, message);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}