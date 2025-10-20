using dotenv.net;
using dotenv.net.Utilities;

namespace Infrastructure;

public class EnvConfig : IConfig
{
    public string BotToken { get; }
    public string UsersPath { get; }

    private EnvConfig(string botToken, string usersPath)
    {
        BotToken = botToken;
        UsersPath = usersPath;
    }

    public static EnvConfig Load()
    {
        DotEnv.Fluent()
            .WithProbeForEnv()
            .Load();

        var botToken = EnvReader.GetStringValue("BOT_TOKEN");
        var usersPath = SaveGetStringValue("USERS_PATH", "Users.json");
        
        return new EnvConfig(botToken, usersPath);
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