using Infrastructure.Config;
using Infrastructure.Exceptions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TeleBot.Commands.Common;
using TeleBot.StateActions.Common;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TeleBot;

public class Bot(
    IConfig config,
    ILogger<Bot> logger,
    CommandHandler commandHandler,
    StateHandler stateHandler) : IHostedService
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
        try
        {
            await TryHandleMessage(bot, message);
        }
        catch (UserFriendlyException ex)
        {
            logger.LogError(ex.ToString());
            await bot.SendMessage(message.Chat, ex.UserMessage);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.ToString());
            await bot.SendMessage(message.Chat,
                $"Упс...произошла непредвиденная ошибка {ex.GetType().Name}. Все вопросы к @feytox");
        }
    }

    private async Task TryHandleMessage(ITelegramBotClient bot, Message message)
    {
        var wasStateHandled = await stateHandler.HandleState(message, bot);
        if (!wasStateHandled) 
            await commandHandler.HandleCommand(message, bot);
    }

    private async Task HandleCallbackQuery(ITelegramBotClient bot, CallbackQuery callbackQuery)
    {
        await HandleMessage(bot, callbackQuery.Message!);
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

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}