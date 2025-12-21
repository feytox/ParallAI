using ParallAI.Core.Services;
using ParallAI.Core.States;
using ParallAI.Core.ValueTypes;
using ParallAI.TeleBot.Core.Callback;
using ParallAI.TeleBot.Core.Callback.Common;
using ParallAI.TeleBot.Core.StateActions;
using ParallAI.TeleBot.Core.Util;
using ParallAI.TeleBot.Services;
using ParallAI.TeleBot.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.StateActions;

public class RequestStateAction(
    IGenerationService genService,
    IMediaGroupCollector groupCollector,
    CancelTokenSourceStorage cancelTokenStorage)
    : StateAction<RequestState>
{
    protected override async Task<bool> Execute(RequestState state, Message message, ITelegramBotClient bot, User user)
    {
        var messages = await groupCollector.CollectMessages(message);
        if (messages is null)
            return false;

        var prompt = AiMessageHelper.CreateAiMessage(messages);

        var cts = new CancellationTokenSource();
        var ctsId = Guid.NewGuid();
        cancelTokenStorage.AddSource(ctsId, cts);

        var sentMessage = await SendProcessingMessage(bot, message.Chat, ctsId);

        try
        {
            await GenerateAndSendResponse(state, prompt, bot, message.Chat, cts.Token);
        }
        catch (TaskCanceledException)
        {
            await bot.SendMessage(message.Chat, "Запрос отменен");
        }
        finally
        {
            cancelTokenStorage.DeleteSource(ctsId);
            await bot.DeleteMessageOptional(sentMessage.Chat, sentMessage.Id);
        }

        if (state.Config.RequestMode == RequestMode.Single)
            user.StateMachine.TryPop();

        return true;
    }

    private async Task<Message> SendProcessingMessage(ITelegramBotClient bot, ChatId chatId, Guid ctsId)
    {
        return await bot.SendMessage(chatId,
            "Ваш запрос отправлен к моделям, ожидайте...",
            replyMarkup: new InlineKeyboardMarkup(
                InlineKeyboardButton.WithCallbackData("Отмена", $"{CancelTaskCallBack.Tag}:{ctsId}")));
    }

    private async Task GenerateAndSendResponse(RequestState state, AiMessage prompt, ITelegramBotClient bot,
        ChatId chatId, CancellationToken token)
    {
        var model = state.Config.Model;
        var preset = state.Config.Preset;
        var settings = preset is not null ? preset.PromptSettings : PromptSettings.Default;

        var response = await genService.Generate(model, [prompt], settings, token);
        await bot.SendMarkdown(chatId, response.Text);
    }

    protected override async Task<bool> ExecuteAfter(RequestState state, ChatId chatId, Message? prevMessage,
        ITelegramBotClient bot, User user)
    {
        var presetText = state.Config.Preset?.Name ?? "не выбран";
        var cancelText = state.Config.RequestMode == RequestMode.Single ? "Отменить" : "Выйти из режима запросов";

        await bot.SendMessage(chatId,
            $"<b>Модель</b> — {state.Config.Model.DisplayName}\n" +
            $"<b>Пресет</b> — {presetText}\n" +
            $"Введи запрос. Также можешь прикрепить файл",
            parseMode: ParseMode.Html,
            replyMarkup: CancelCallback.CreateMarkup(cancelText));

        return true;
    }
}