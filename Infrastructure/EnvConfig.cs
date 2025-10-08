using dotenv.net;
using dotenv.net.Utilities;

namespace ParallAI.Infrastructure;


public class EnvConfig
{
    public static EnvConfig Instance { get; } = Load();
    
    public string BotToken { get; }
    
    private EnvConfig(string botToken)
    {
        BotToken = botToken;
    }

    private static EnvConfig Load()
    {
        DotEnv.Fluent()
            .WithProbeForEnv()
            .Load();
            
        var botToken = EnvReader.GetStringValue("BOT_TOKEN");
        return new EnvConfig(botToken);
    }
}