namespace Infrastructure;

public interface IConfig
{
    public string BotToken { get; }
    
    public string UsersPath { get; }
}