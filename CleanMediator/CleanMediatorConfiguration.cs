using System.Reflection;

namespace CleanMediator;

public class CleanMediatorConfiguration
{
    internal List<Assembly> RegisteredAssemblies { get; } = [];

    public CleanMediatorConfiguration RegisterAssembly(Assembly assembly)
    {
        RegisteredAssemblies.Add(assembly);
        return this;
    }

    public CleanMediatorConfiguration RegisterAssemblies(params Assembly[] assemblies)
    {
        RegisteredAssemblies.AddRange(assemblies);
        return this;
    }
}