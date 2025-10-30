using dotenv.net;
using dotenv.net.Utilities;

namespace Infrastructure;

public class EnvConfig : IConfig
{
    public string BotToken { get; }
    public string MongoConnectionString { get; }
    public string UsersCollection { get; }

    private EnvConfig(string botToken, string mongoConnectionString, string usersCollection)
    {
        BotToken = botToken;
        MongoConnectionString = mongoConnectionString;
        UsersCollection = usersCollection;
    }

    public static EnvConfig Load()
    {
        DotEnv.Fluent()
            .WithProbeForEnv()
            .Load();

        var botToken = EnvReader.GetStringValue("BOT_TOKEN");
        var mongoConnectionString = SaveGetStringValue("MONGO_CONNECTION_STRING", "mongodb://localhost:27017" );
        var usersCollection = SaveGetStringValue("USERS_COLLECTION", "Users");
        
        return new EnvConfig(botToken, mongoConnectionString, usersCollection);
    }

    private static string SaveGetStringValue(string key, string defaultValue)
    {
        try
        {
            return EnvReader.GetStringValue(key);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }
}