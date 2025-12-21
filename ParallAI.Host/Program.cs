using Microsoft.Extensions.Hosting;
using ParallAI.Core;
using ParallAI.Infrastructure;
using ParallAI.TeleBot;
using ParallAI.TeleBot.Core;

namespace ParallAI.Host;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddCore();
                services.AddTelebotCore();
                services.AddInfrastructure();
                services.AddTeleBot();
            })
            .Build();

        await host.RunAsync();
    }
}