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
    }

    public static EnvConfig Load()
    {
        DotEnv.Fluent()
            .WithProbeForEnv()
            .Load();

        var botToken = EnvReader.GetStringValue("BOT_TOKEN");
        var usersPath = EnvReader.GetStringValue("USERS_PATH");
        
        return new EnvConfig(botToken, usersPath);
    }
}