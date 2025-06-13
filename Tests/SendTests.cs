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
    public Task<CanFail<Pong>> Handle(Ping query, CancellationToken cancellationToken)
    {
        var result = new CanFail<Pong>();
        result.Succeeded(new Pong(query.Value + "Pong"));
        return Task.FromResult(result);
    }
}

public class VoidPing : IRequest
{
    public bool Called { get; set; }
}

public class VoidPingHandler : IRequestHandler<VoidPing>
{
    public Task<CanFail> Handle(VoidPing query, CancellationToken cancellationToken)
    {
        query.Called = true;
        return Task.FromResult(CanFail.Success);
    }
}

public record ScopedPong(Guid ScopeId);
public record ScopedPing : IRequest<ScopedPong>;

public class ScopedPingHandler(IServiceProvider serviceProvider): IRequestHandler<ScopedPing, ScopedPong>
{
    public Task<CanFail<ScopedPong>> Handle(ScopedPing query, CancellationToken cancellationToken)
    {
        var scopedService = serviceProvider.GetRequiredService<ScopedService>();
        
        var result = new CanFail<ScopedPong>();
        result.Succeeded(new ScopedPong(scopedService.ScopeId));
        return Task.FromResult(result);
    }
}

public record ScopedVoidPing : IRequest;

public class ScopedVoidPingHandler(IServiceProvider serviceProvider) : IRequestHandler<ScopedVoidPing>
{
    public Task<CanFail> Handle(ScopedVoidPing query, CancellationToken cancellationToken)
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
}