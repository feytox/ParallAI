namespace ParallAI.Core;

public interface IConfig
{
    public string BotToken { get; }

    public string MongoConnectionString { get; }

    public string UsersCollection { get; }
}