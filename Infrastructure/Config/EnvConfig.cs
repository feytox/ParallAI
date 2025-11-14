using dotenv.net;
using dotenv.net.Utilities;

namespace Infrastructure.Config;

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
        var mongoConnectionString = GetOrDefaultString("MONGO_CONNECTION_STRING", "mongodb://localhost:27017");
        var usersCollection = GetOrDefaultString("USERS_COLLECTION", "Users");

        return new EnvConfig(botToken, mongoConnectionString, usersCollection);
    }

    private static string GetOrDefaultString(string key, string defaultValue)
    {
        return EnvReader.TryGetStringValue(key, out var result) ? result! : defaultValue;
    }
}