using AICore.Repositories;
using AICore.Services;
using AICore.ValueTypes;
using TeleBot.Commands.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;

namespace TeleBot.Example.Commands;

// временная команда для отправки запросоов с выбранным пресетом
// Ввод: /presetrequest Название пресета
[Command("/presetrequest", "отправляет запрос с указанным пресетом")]
public class PresetRequestCommand(IRepository<User, long> users, GenerationService genService) : ICommand
{
    public async Task Execute(Message message, ITelegramBotClient bot)
    {
        var userId = message.From!.Id;
        var user = await users.GetById(userId);
        var model = user!.UserModels.First();
        var presetName = message.Text!.Substring("/presetrequest".Length).Trim(' ');
        if (presetName.Length == 0)
        {
            await bot.SendMessage(message.Chat, "Введите название пресета");
            return;
        }

        var preset = user.UserPresets.FirstOrDefault(p =>
            string.Equals(p.Name, presetName, StringComparison.CurrentCultureIgnoreCase));
        if (preset is null)
        {
            await bot.SendMessage(message.Chat, "Вы не добавляли пресет с таким названием");
            return;
        }
        
        var prompt = new Prompt("Какая сейчас погода в Новой Зеландии?");
        var promptSettings = new PromptSettings(preset.SystemPrompt, preset.Temperature, preset.ThinkingBudget);
        var response = await genService.Generate(model, prompt, promptSettings);

        await bot.SendMessage(message.Chat, response.Text);
    }
}