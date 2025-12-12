using ParallAI.Core.Services;
using ParallAI.Core.States;
using ParallAI.Core.ValueTypes;
using ParallAI.TeleBot.Core.Callback;
using ParallAI.TeleBot.Core.StateActions;
using ParallAI.TeleBot.Services;
using ParallAI.TeleBot.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.StateActions;

public class RequestStateAction(GenerationService genService, MediaGroupCollector groupCollector) : StateAction<RequestState>
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

        var response = await genService.Generate(model!, [prompt], settings);
        await bot.SendMessage(message.Chat, response.Text);
        if (state.Config.RequestMode == RequestMode.Single)
            user.StateMachine.TryPop();
        return true;
    }

    protected override async Task<bool> ExecuteAfter(RequestState state, ChatId chatId, ITelegramBotClient bot, User user)
    {
        await bot.SendMessage(chatId, 
            "<b>Модель</b> (/models) — " + state.Config.Model.DisplayName + 
            "\n<b>Пресет</b> (/presets) — " + (state.Config.Preset is not null ? state.Config.Preset.Name : "не выбран") + 
            "\nВведи запрос. Также можешь прикрепить файл", ParseMode.Html,
            replyMarkup: CancelCallback.CreateMarkup(
                state.Config.RequestMode == RequestMode.Single
                    ? "Отменить"
                    : "Выйти из режима запросов"));
        return true;
    }
}