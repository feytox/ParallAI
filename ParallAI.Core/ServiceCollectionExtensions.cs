using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ParallAI.Core.Services;

namespace ParallAI.Core;

public static class ServiceCollectionExtensions
{
    public static void AddCore(this IServiceCollection services)
    {
        services.AddSingleton<GenerationService>();
    }

    public static void AddScannedHandlers<TInterface>( // мб отдельный класс создать 
        this IServiceCollection services,
        Assembly assembly,
        params Type[] attributeTypes)
    {
        var implementationTypes = assembly.GetTypes()
            .Where(t => typeof(TInterface).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false })
            .ToList();
        
        foreach (var type in implementationTypes)
            services.AddSingleton(typeof(TInterface), type);
        
        foreach (var attrType in attributeTypes)
        {
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(attrType);

            services.AddSingleton(enumerableType, _ =>
            {
                var attributes = implementationTypes
                    .SelectMany(t => t.GetCustomAttributes(attrType, false))
                    .ToList();

                var typedArray = Array.CreateInstance(attrType, attributes.Count);
                for (var i = 0; i < attributes.Count; i++)
                    typedArray.SetValue(attributes[i], i);

                return typedArray;
            });
        }
    }
}