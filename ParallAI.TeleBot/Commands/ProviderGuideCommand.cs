using ParallAI.Core.Repositories;
using ParallAI.TeleBot.Commands.UI;
using ParallAI.TeleBot.Core.Commands.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ParallAI.TeleBot.Commands;

[Command("/providerguide", "гайд на получение токена для провайдера")]
public class ProviderGuideCommand : ICommand
{
    private const string GuideUrl = "https://github.com/feytox/ParallAI/blob/add-more-ui/ProviderGuide/PROVIDERGUIDE.md";

    public async Task Execute(Message message, ITelegramBotClient bot)
    {
        const string text = "📚 <b>Гайд по настройке провайдеров</b>\n\n" +
                            "В этом гайде подробно написано как получить токены для разных провайдеров.\n\n" +
                            $"👉 <a href='{GuideUrl}'><b>тык :3</b></a>";

        await bot.SendMessage(
            chatId: message.Chat.Id,
            text: text,
            parseMode: ParseMode.Html,
            linkPreviewOptions: new LinkPreviewOptions { IsDisabled = true }
        );
    }
}