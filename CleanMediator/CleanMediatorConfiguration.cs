using System.Reflection;

namespace CleanMediator;

public class CleanMediatorConfiguration
{
    internal List<Assembly> RegisteredAssemblies { get; } = [];

    /// <summary>
    /// Register handlers from assembly <paramref name="assembly"/>
    /// </summary>
    public CleanMediatorConfiguration RegisterServicesFromAssembly(Assembly assembly)
    {
        RegisteredAssemblies.Add(assembly);
        return this;
    }

    /// <summary>
    /// Register handlers from list of <paramref name="assemblies"/>
    /// </summary>
    public CleanMediatorConfiguration RegisterServicesFromAssemblies(params Assembly[] assemblies)
    {
        RegisteredAssemblies.AddRange(assemblies);
        return this;
    }
}