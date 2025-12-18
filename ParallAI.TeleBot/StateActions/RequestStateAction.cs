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

public class RequestStateAction(GenerationService genService, MediaGroupCollector groupCollector, 
    CancelTokenSourceStorage cancelTokenStorage) 
    : StateAction<RequestState>
{
    protected override async Task<bool> Execute(RequestState state, Message message, ITelegramBotClient bot, User user)
    {
        var messages = await groupCollector.CollectMessages(message);
        if (messages is null)
            return false;

        var model = state.Config.Model;
        var preset = state.Config.Preset;
        var prompt = AiMessageHelper.CreateAiMessage(messages);
        var settings = preset is not null ? preset.PromptSettings : PromptSettings.Default;
        
        var cts = new CancellationTokenSource();
        var ctsId = Guid.NewGuid();
        cancelTokenStorage.AddSource(ctsId, cts);
        
        var sentMessage = await bot.SendMessage(message.Chat, 
            "Ваш запрос отправлен к моделям, ожидайте...",
            replyMarkup: new InlineKeyboardMarkup(
                InlineKeyboardButton.WithCallbackData("Отмена", $"{CancelTaskCallBack.Tag}:{ctsId}")));
        try
        {
            var response = await genService.Generate(model, [prompt], settings, cts.Token);
            await bot.SendMarkdown(message.Chat, response.Text);
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

    protected override async Task<bool> ExecuteAfter(RequestState state, ChatId chatId, Message? prevMessage, 
        ITelegramBotClient bot, User user)
    {
        if (prevMessage is not null)
            await bot.DeleteMessageOptional(chatId, prevMessage.Id);
        
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