using dotenv.net;
using dotenv.net.Utilities;

namespace Infrastructure;

public class EnvConfig : IConfig
{
    public string BotToken { get; }

    private EnvConfig(string botToken)
    {
        BotToken = botToken;
    }

    public static EnvConfig Load()
    {
        DotEnv.Fluent()
            .WithProbeForEnv()
            .Load();

        var botToken = EnvReader.GetStringValue("BOT_TOKEN");
        return new EnvConfig(botToken);
    }
}