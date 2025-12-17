using ParallAI.MarkdownV2;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ParallAI.TeleBot.Core.Util;

public static class TelegramBotExtensions
{
    public static async Task<Message> EditCallbackMessage(this ITelegramBotClient bot, CallbackQuery query,
        string text, ParseMode parseMode = ParseMode.None, InlineKeyboardMarkup? replyMarkup = null)
    {
        var message = query.GetMessage();
        return await bot.EditMessageText(message.Chat, message.Id, text,
            parseMode: parseMode, replyMarkup: replyMarkup);
    }

    public static async Task DeleteCallbackMessage(this ITelegramBotClient bot, CallbackQuery query)
    {
        var message = query.GetMessage();
        await bot.DeleteMessage(message.Chat, message.Id);
    }

    public static async Task DeleteMessageOptional(this ITelegramBotClient bot, ChatId chatId, int messageId)
    {
        await bot.DeleteMessages(chatId, [messageId]);
    }

    public static async Task SendMarkdown(this ITelegramBotClient bot, ChatId chatId, string message)
    {
        var converter = new MarkdownV2Converter();

        var convertedText = converter.Convert(message);
        var parts = convertedText.SplitMessages();

        foreach (var part in parts)
            await bot.SendMessage(
                chatId: chatId,
                text: part,
                parseMode: ParseMode.MarkdownV2,
                linkPreviewOptions: new LinkPreviewOptions { IsDisabled = true }
            );
    }
}