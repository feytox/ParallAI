using System.Collections.Concurrent;
using ParallAI.TeleBot.Util;
using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Services;

public class MediaGroupCollector
{
    private static readonly ConcurrentDictionary<string, List<Message>> MediaGroups = new();
    
    public async Task<Message[]?> CollectMessages(Message message)
    {
        if (!message.IsMediaGroup())
            return [message];
    
        var groupId = message.MediaGroupId;
        var isFirstMessage = false;
        
        MediaGroups.AddOrUpdate(
            key: groupId!,
            addValueFactory: _ => 
            {
                isFirstMessage = true;
                return [message];
            },
            updateValueFactory: (_, list) => 
            {
                lock (list)
                {
                    list.Add(message);
                }
                return list;
            });
    
        if (isFirstMessage)
        {
            await Task.Delay(1000);
            
            if (MediaGroups.TryRemove(groupId!, out var list))
            {
                return list.OrderBy(m => m.MessageId).ToArray();
            }
        }
    
        return null;
    }
}