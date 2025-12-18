using ParallAI.Core.Services;
using ParallAI.Core.States;
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

public class CompareStateAction(ComparisonService compareService, MediaGroupCollector groupCollector, 
    CancelTokenSourceStorage cancelTokenStorage)
    : StateAction<CompareState>
{
    protected override async Task<bool> Execute(CompareState state, Message message, ITelegramBotClient bot, User user)
    {
        var messages = await groupCollector.CollectMessages(message);
        if (messages is null)
            return false;

        var aiMessage = AiMessageHelper.CreateAiMessage(messages);
        
        var cts = new CancellationTokenSource();
        var ctsId = Guid.NewGuid();
        cancelTokenStorage.AddSource(ctsId, cts);
        
        var sentMessage = await bot.SendMessage(message.Chat, 
            "Ваш запрос отправлен к моделям, ожидайте...",
            replyMarkup: new InlineKeyboardMarkup(
                InlineKeyboardButton.WithCallbackData("Отмена", $"{CancelTaskCallBack.Tag}:{ctsId}")));

        try
        {
            var result = await compareService.Generate(aiMessage, state.Config, cts.Token);

            for (var i = 0; i < result.Responses.Length; i++)
            {
                var response = result.Responses[i];
                await bot.SendMarkdown(message.Chat, $"**Ответ {i + 1} модели:**\n\n{response.Text}");
            }

            await bot.SendMarkdown(message.Chat, $"**Ответ оркестратора:**\n\n{result.OrchestratorResponse.Text}");
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
        return true;
    }

    protected override async Task<bool> ExecuteAfter(CompareState state, ChatId chatId, Message? prevMessage,
        ITelegramBotClient bot, User user)
    {
        // TODO: change text
        await bot.SendMessage(chatId, "Введи новый запрос. Также можешь прикрепить файл",
            replyMarkup: CancelCallback.CreateMarkup("Выйти из режима сравнения"));
        return true;
    }
}