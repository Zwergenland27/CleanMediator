using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace CleanMediator;

/// <summary>
/// Extensions to add mediator to services
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers mediator types and handlers from the in the configuration specified assemblies
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">The action used to configure the mediator options</param>
    public static IServiceCollection AddCleanMediator(this IServiceCollection services, Action<CleanMediatorConfiguration> configuration)
    {
        var configurationBuilder = new CleanMediatorConfiguration();
        configuration(configurationBuilder);

        services.AddTransient<IMediator, Mediator>();
        
        configurationBuilder.RegisteredAssemblies.ForEach(assembly =>
        {
            services.RegisterHandlers(assembly, typeof(IRequestHandler<>));
            services.RegisterHandlers(assembly, typeof(IRequestHandler<,>));
        });
        
        return services;
    }

    private static IServiceCollection RegisterHandlers(this IServiceCollection services, Assembly assembly, Type handlerType)
    {
        var handlerTypes = assembly.GetTypes()
            .Where(type => !type.IsAbstract && !type.IsInterface)
            .SelectMany(type => type.GetInterfaces(), (type, @interface) => new { type, @interface })
            .Where(t => t.@interface.IsGenericType && t.@interface.GetGenericTypeDefinition() == handlerType);

        foreach (var handler in handlerTypes)
        {
            services.AddTransient(handler.@interface, handler.type);
        }

        return services;
    }
}