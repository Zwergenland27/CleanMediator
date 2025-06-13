using System.Net.NetworkInformation;
using System.Reflection;
using CleanDomainValidation.Domain;
using CleanMediator;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Tests;

public record Pong(string Value);

public record Ping(string Value) : IRequest<Pong>;

public class PingHandler : IRequestHandler<Ping, Pong>
{
    public async Task<CanFail<Pong>> Handle(Ping command, CancellationToken cancellationToken)
    {
        return new Pong(command.Value + "Pong");
    }
}

public class VoidPing : IRequest
{
    public bool Called { get; set; }
}

public class VoidPingHandler : IRequestHandler<VoidPing>
{
    public Task<CanFail> Handle(VoidPing @event, CancellationToken cancellationToken)
    {
        @event.Called = true;
        return Task.FromResult(CanFail.Success);
    }
}

public record ScopedPong(Guid ScopeId);
public record ScopedPing : IRequest<ScopedPong>;

public class ScopedPingHandler(IServiceProvider serviceProvider): IRequestHandler<ScopedPing, ScopedPong>
{
    public async Task<CanFail<ScopedPong>> Handle(ScopedPing command, CancellationToken cancellationToken)
    {
        var scopedService = serviceProvider.GetRequiredService<ScopedService>();
        return new ScopedPong(scopedService.ScopeId);
    }
}

public record ScopedVoidPing : IRequest;

public class ScopedVoidPingHandler(IServiceProvider serviceProvider) : IRequestHandler<ScopedVoidPing>
{
    public Task<CanFail> Handle(ScopedVoidPing @event, CancellationToken cancellationToken)
    {
        var scopedService = serviceProvider.GetRequiredService<ScopedService>();
        var singletonService = serviceProvider.GetRequiredService<SingletonScopedServiceTest>();
        singletonService.InnerScopeId = scopedService.ScopeId;
        return Task.FromResult(CanFail.Success);
    }
}

public class ScopedService
{
    public Guid ScopeId { get; } = Guid.NewGuid();
}

public class SingletonScopedServiceTest
{
    public Guid OuterScopeId { get; set; }
    public Guid InnerScopeId { get; set; }
}

public class SendTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMediator _mediator;
    public SendTests()
    {
        var services = new ServiceCollection();
        services.AddCleanMediator(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        
        services.AddScoped<ScopedService>();
        services.AddSingleton<SingletonScopedServiceTest>();
        
        _serviceProvider = services.BuildServiceProvider();
        _mediator = _serviceProvider.GetService<IMediator>()!;
    }
    
    [Fact]
    public async Task SendAsync_ShouldReturnResult_WhenNoError()
    {
        //Arrange
        var ping = new VoidPing
        {
            Called = false
        };
        
        //Act
        var result = await _mediator.SendAsync(ping);
        
        //Assert
        result.HasFailed.ShouldBeFalse();
        ping.Called.ShouldBeTrue();
    }
    
    [Fact]
    public async Task SendInOwnScopeAsync_ShouldReturnResult_WhenNoError()
    {
        //Arrange
        var ping = new VoidPing
        {
            Called = false
        };
        
        //Act
        var result = await _mediator.SendInOwnScopeAsync(ping);
        
        //Assert
        result.HasFailed.ShouldBeFalse();
        ping.Called.ShouldBeTrue();
    }
    
    [Fact]
    public async Task SendInOwnScopeAsync_ShouldRunInNewScope()
    {
        //Arrange
        var ping = new ScopedVoidPing();
        
        var scopeId = _serviceProvider.GetRequiredService<ScopedService>().ScopeId;
        var singletonService = _serviceProvider.GetRequiredService<SingletonScopedServiceTest>();
        singletonService.OuterScopeId = scopeId;
        
        //Act
        _ = await _mediator.SendInOwnScopeAsync(ping);
        
        //Assert
        
        singletonService.OuterScopeId.ShouldNotBe(singletonService.InnerScopeId);
    }
    
    [Fact]
    public async Task SendWithResultAsync_ShouldReturnResult_WhenNoError()
    {
        //Arrange
        var message = "Ping";
        var ping = new Ping(message);
        
        //Act
        var result = await _mediator.SendAsync(ping);
        
        //Assert
        result.HasFailed.ShouldBeFalse();
        var pong = result.Value;
        pong.ShouldBeOfType<Pong>();
        pong.Value.ShouldBe(message + "Pong");
    }
    
    [Fact]
    public async Task SendInOwnScopeWithResultAsync_ShouldReturnResult_WhenNoError()
    {
        //Arrange
        var message = "Ping";
        var ping = new Ping(message);
        
        //Act
        var result = await _mediator.SendInOwnScopeAsync(ping);
        
        //Assert
        result.HasFailed.ShouldBeFalse();
        var pong = result.Value;
        pong.ShouldBeOfType<Pong>();
        pong.Value.ShouldBe(message + "Pong");
    }
    
    [Fact]
    public async Task SendInOwnScopeWithResultAsync_ShouldRunInNewScope()
    {
        //Arrange
        var ping = new ScopedPing();
        
        var scopeId = _serviceProvider.GetRequiredService<ScopedService>().ScopeId;
        
        //Act
        var result = await _mediator.SendInOwnScopeAsync(ping);
        
        //Assert
        var scopeInRequestId = result.Value.ScopeId;
        
        scopeId.ShouldNotBe(scopeInRequestId);
    }
}