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
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.StateActions;

public class CompareStateAction(
    ComparisonService compareService,
    IMediaGroupCollector groupCollector,
    CancelTokenSourceStorage cancelTokenStorage)
    : StateAction<CompareState>
{
    protected override async Task<ActionResult> Execute(CompareState state, Message message, ITelegramBotClient bot, User user)
    {
        var messages = await groupCollector.CollectMessages(message);
        if (messages is null)
            return ActionResult.HandledCompletely;

        var aiMessage = AiMessageHelper.CreateAiMessage(messages);

        var cts = new CancellationTokenSource();
        var ctsId = Guid.NewGuid();
        cancelTokenStorage.AddSource(ctsId, cts);

        var sentMessage = await SendProcessingMessage(bot, message.Chat, ctsId);

        try
        {
            await GenerateAndSendResponses(state, aiMessage, bot, message.Chat, cts.Token);
        }
        catch (TaskCanceledException)
        {
            await bot.SendMessage(message.Chat, "Запросы отменены");
        }
        finally
        {
            cancelTokenStorage.DeleteSource(ctsId);
            await bot.DeleteMessageOptional(sentMessage.Chat, sentMessage.Id);
        }

        return ActionResult.Handled;
    }

    private async Task<Message> SendProcessingMessage(ITelegramBotClient bot, ChatId chatId, Guid ctsId)
    {
        return await bot.SendMessage(chatId,
            "Ваш запрос отправлен к моделям, ожидайте...",
            replyMarkup: new InlineKeyboardMarkup(
                InlineKeyboardButton.WithCallbackData("Отмена", $"{CancelTaskCallBack.Tag}:{ctsId}")));
    }

    private async Task GenerateAndSendResponses(CompareState state, AiMessage aiMessage, ITelegramBotClient bot,
        ChatId chatId, CancellationToken token)
    {
        var result = await compareService.Generate(aiMessage, state.Config, token);

        for (var i = 0; i < result.Responses.Length; i++)
        {
            var response = result.Responses[i];
            await bot.SendMarkdown(chatId, $"**Ответ {i + 1} модели:**\n\n{response.Text}");
        }

        await bot.SendMarkdown(chatId, $"**Ответ оркестратора:**\n\n{result.OrchestratorResponse.Text}");
    }

    protected override async Task<ActionResult> ExecuteAfter(CompareState state, ChatId chatId, Message? prevMessage,
        ITelegramBotClient bot, User user)
    {
        await bot.SendMessage(chatId, "Введи новый запрос. Также можешь прикрепить файл",
            replyMarkup: CancelCallback.CreateMarkup("Выйти из режима сравнения"));
        return ActionResult.Handled;
    }
}