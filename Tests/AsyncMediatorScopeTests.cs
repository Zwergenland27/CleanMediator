using System.Reflection;
using CleanMediator;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Tests;

public class AsyncMediatorScopeTests
{
    
    [Fact]
    public async Task SendAsync_ShouldCallImplementation()
    {
        //Arrange
        var ping = new VoidPing();
        
        var mediatorImplementation = Substitute.For<IMediator>();
        
        var serviceProvider = Substitute.For<IServiceProvider>();
        serviceProvider.GetService(typeof(IMediator)).Returns(mediatorImplementation);
        
        var serviceScope = Substitute.For<IServiceScope>();
        serviceScope.ServiceProvider.Returns(serviceProvider);
        
        await using var asyncServiceScope = new AsyncServiceScope(serviceScope);
        await using var scopedMediator = new AsyncMediatorScope(asyncServiceScope);
        
        //Act
        _ = await scopedMediator.SendAsync(ping);
        
        //Assert
        await mediatorImplementation.Received(1).SendAsync(ping);
    }
    
    [Fact]
    public async Task SendWithResultAsync_ShouldCallImplementation()
    {
        //Arrange
        var message = "Ping";
        var ping = new Ping(message);
        
        var mediatorImplementation = Substitute.For<IMediator>();
        
        var serviceProvider = Substitute.For<IServiceProvider>();
        serviceProvider.GetService(typeof(IMediator)).Returns(mediatorImplementation);
        
        var serviceScope = Substitute.For<IServiceScope>();
        serviceScope.ServiceProvider.Returns(serviceProvider);
        
        await using var asyncServiceScope = new AsyncServiceScope(serviceScope);
        await using var scopedMediator = new AsyncMediatorScope(asyncServiceScope);
        
        //Act
        _ = await scopedMediator.SendAsync(ping);
        
        //Assert
        await mediatorImplementation.Received(1).SendAsync(ping);
    }
}