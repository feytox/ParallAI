using ParallAI.TeleBot.Core.Commands.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ParallAI.TeleBot.Commands;

[Command("/providerguide", "гайд на получение токена для провайдера")]
public class ProviderGuideCommand : ICommand
{
    private const string GuideUrl = "https://github.com/feytox/ParallAI/blob/main/ProviderGuide/PROVIDERGUIDE.md";

    public const string GuideHtmlUrl = $"<a href='{GuideUrl}'><b>*тык*</b></a>";
    
    public async Task Execute(ChatId chatId, long userId, ITelegramBotClient bot)
    {
        const string text = "📚 <b>Гайд по настройке провайдеров</b>\n\n" +
                            "В этом гайде подробно написано как получить токены для разных провайдеров.\n\n" +
                            $"👉 {GuideHtmlUrl}";

        await bot.SendMessage(
            chatId: chatId,
            text: text,
            parseMode: ParseMode.Html,
            linkPreviewOptions: new LinkPreviewOptions { IsDisabled = true }
        );
    }
}