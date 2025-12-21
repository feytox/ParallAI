using System.Collections.Concurrent;
using ParallAI.TeleBot.Util;
using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Services;

public class MediaGroupCollector : IMediaGroupCollector
{
    private const int AllMessagesCooldown = 1000;

    private readonly ConcurrentDictionary<string, ConcurrentBag<Message>> mediaGroups = new();

    public async Task<Message[]?> CollectMessages(Message message)
    {
        if (!message.IsMediaGroup())
            return [message];

        var groupId = message.MediaGroupId!;
        var isFirstMessage = AddMessage(groupId, message);

        if (!isFirstMessage)
            return null;

        await Task.Delay(AllMessagesCooldown);
        return mediaGroups.TryRemove(groupId, out var messages)
            ? messages.OrderBy(m => m.MessageId).ToArray()
            : null;
    }

    private bool AddMessage(string groupId, Message message)
    {
        var isFirstMessage = !mediaGroups.ContainsKey(groupId);

        mediaGroups.AddOrUpdate(
            key: groupId,
            addValueFactory: _ => [message],
            updateValueFactory: (_, bag) =>
            {
                bag.Add(message);
                return bag;
            });

        return isFirstMessage;
    }
}