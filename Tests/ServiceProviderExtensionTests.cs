using System.Reflection;
using CleanMediator;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Tests;

public class ServiceProviderExtensionTests
{
    [Fact]
    public async Task CreateAsyncMediatorScope_Should_ReturnAsyncMediatorScopeInNewAsyncScope()
    {
        //Arrange
        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();
        
        //Act
        await using var asyncMediatorScopeA = provider.CreateAsyncMediatorScope();
        await using var asyncMediatorScopeB = provider.CreateAsyncMediatorScope();

        //Assert
        var scopeField = typeof(AsyncMediatorScope).GetField("<serviceScope>P", BindingFlags.NonPublic | BindingFlags.Instance);
        var scopeFieldA = scopeField!.GetValue(asyncMediatorScopeA);
        var scopeFieldB = scopeField.GetValue(asyncMediatorScopeB);
        
        scopeFieldA.ShouldNotBe(scopeFieldB);
    }
}