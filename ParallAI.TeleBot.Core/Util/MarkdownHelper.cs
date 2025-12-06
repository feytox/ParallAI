using ParallAI.MarkdownV2;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ParallAI.TeleBot.Core.Util;

public static class MarkdownHelper
{
    public static Task<Message> SendMarkdown(this ITelegramBotClient bot, ChatId chatId, string message)
    {
        var text = MarkdownV2Converter.Convert(message);
        // TODO: использовать ParseMode.MarkdownV2 после реализации конвертера (issue #58)
        return bot.SendMessage(chatId, text, parseMode: ParseMode.None);
    }
}