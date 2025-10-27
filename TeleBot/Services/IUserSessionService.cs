namespace TeleBot.Services;

public interface IUserSessionService
{
    Task<string> GetCommandToExecuteName(long chatId, string messageText);
    
    Task UpdateSession(long chatId, NextCommandInfo? nextCommandInfo);
}