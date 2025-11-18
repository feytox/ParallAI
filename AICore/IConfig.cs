namespace AICore;

public interface IConfig
{
    public string BotToken { get; }
    
    public string MongoConnectionString { get; }
    
    public string UsersCollection { get; }
}