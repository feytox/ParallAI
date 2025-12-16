using ParallAI.Core.Repositories;
using ParallAI.TeleBot.Callback;
using ParallAI.TeleBot.Commands.UI;
using ParallAI.TeleBot.Core.Commands.Common;
using ParallAI.TeleBot.Core.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Commands;

[MainMenu("🤖 Модели", MenuOrder.Models)]
[Command("/models", "список и конфигурация моделей")]
public class ModelsCommand(IRepository<User, long> users) : UserCommand(users)
{
    protected override async Task Execute(ChatId chatId, ITelegramBotClient bot, User user)
    {
        await bot.SendMarkdown(chatId, @"Идём **справа налево** и ищем первую цифру, которая **меньше** следующей справа.

```
1 4 3 2
      ↑
     3>2 — убывает
  ↑
  4>3 — убывает
↑
1<4 — нашли место i=0
```");
    }
}