using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ParallAI.Core;
using ParallAI.Core.Exceptions;
using ParallAI.TeleBot.Core.Callback;
using ParallAI.TeleBot.Core.Commands;
using ParallAI.TeleBot.Core.StateActions;
using ParallAI.TeleBot.Util;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ParallAI.TeleBot;

public class Bot(
    IConfig config,
    ILogger<Bot> logger,
    CommandHandler commandHandler,
    StateHandler stateHandler,
    CallbackQueryHandler callbackQueryHandler) : IHostedService
{
    public ITelegramBotClient Client => client ?? throw new NullReferenceException("Bot is not initialized");

    private TelegramBotClient? client;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        client = new TelegramBotClient(config.BotToken, cancellationToken: cancellationToken);

        client.StartReceiving(HandleUpdate, HandleError, cancellationToken: cancellationToken);

        var commands = commandHandler
            .CommandsDescription
            .Select(attr => new BotCommand(attr.Name, attr.Description))
            .ToArray();
        
        await client.SetMyCommands(commands, cancellationToken: cancellationToken);
        
        logger.LogInformation("Bot has been started.");
    }

    private Task HandleUpdate(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        // TODO: maybe change (issue #48)
        Task.Run(async () =>
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
                    $"Упс...произошла непредвиденная ошибка {ex.GetType().Name}");
            }
        }, cancellationToken);
        return Task.CompletedTask;
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
        var mainHandled = await stateHandler.HandleState(message, bot);
        var postHandled = await stateHandler.HandlePostState(message.From!.Id, bot);
        if (!mainHandled && !postHandled)
            await commandHandler.HandleCommand(message, bot);
    }

    private async Task HandleCallbackQuery(ITelegramBotClient bot, CallbackQuery callbackQuery)
    {
        await callbackQueryHandler.HandleCallbackQuery(callbackQuery, bot);
        await stateHandler.HandlePostState(callbackQuery.From.Id, bot);
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