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