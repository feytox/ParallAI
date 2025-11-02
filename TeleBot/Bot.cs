#region

using AICore.Repositories;
using User = AICore.Entities.User;
using Infrastructure.Config;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

#endregion

namespace TeleBot;

public class Bot(
    IConfig config,
    ILogger<Bot> logger,
    IRepository<User, long> userRepository,
    CommandHandler commandHandler,
    StateHandler stateHandler) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var bot = new TelegramBotClient(config.BotToken, cancellationToken: cancellationToken);

        bot.StartReceiving(HandleUpdate, HandleError, cancellationToken: cancellationToken);
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
        var user = await userRepository.GetOrCreate(message.Chat.Id);
        var wasState = await stateHandler.HandleState(message, bot, user);
        if (!wasState) await commandHandler.HandleCommand(message, bot, user);
        await userRepository.Update(user);
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