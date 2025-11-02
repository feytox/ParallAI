using AICore.Repositories;
using AICore.Services;
using AICore.ValueTypes;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;

namespace TeleBot.Commands;

[Command("/request", "тестовая команда для запросов к модели")]
public class RequestCommand(IRepository<User, long> users, IAiGenerationService<GeminiProvider> genService) : ICommand
{
    public async Task Execute(Message message, ITelegramBotClient bot)
    {
        var userId = message.From!.Id;
        var user = await users.GetById(userId);
        var model = user!.Models.First();
        var provider = (GeminiProvider)model.Provider;

        var prompt = new Prompt("What is the capital of France?");
        var response = await genService.Generate(provider, model.ModelId, prompt, PromptSettings.Default);
        
        await bot.SendMessage(message.Chat, response.Text);
    }
}