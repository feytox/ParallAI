using System.Reflection;
using Infrastructure;
using AICore;
using AICore.States;

namespace TeleBot.Services;

public class UserSessionService(IRepository<User, long> userRepository) : IUserSessionService
{
    public async Task<string> GetCommandToExecuteName(long chatId, string messageText)
    {
        await userRepository.TryGetById(chatId, out var user);

        if (!messageText.StartsWith($"/") && user?.State?.NextCommandName is not null)
            return user.State.NextCommandName;
        
        return messageText.Split(' ')[0];
    }

    public async Task UpdateSession(long chatId, NextCommandInfo? nextCommandInfo)
    {
        await userRepository.TryGetById(chatId, out var user);
        
        var nextCommandName = nextCommandInfo?.NextCommandName;
        
        if (nextCommandName is null)
        {
            user.State = null;
            await userRepository.TryUpdate(user);
            return;
        }

        user.State = new DefaultUserState { NextCommandName = nextCommandName };
        await userRepository.TryUpdate(user);
    }
}