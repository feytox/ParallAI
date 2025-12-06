using Microsoft.Extensions.DependencyInjection;
using ParallAI.Core.Services;

namespace ParallAI.Core;

public static class ServiceCollectionExtensions
{
    public static void AddCore(this IServiceCollection services)
    {
        services.AddSingleton<GenerationService>();
        services.AddSingleton<ComparisonService>();
    }
}