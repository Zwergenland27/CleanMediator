using System.Reflection;
using CleanDomainValidation.Domain;
using CleanMediator;
using CleanMediator.Commands;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using ICommand = CleanMediator.Commands.ICommand;

namespace Tests.Commands;

public record Pong(string Value);

public record Ping(string Value) : ICommand<Pong>;

public class PingHandler : CommandHandler<Ping, Pong>
{
    public override async Task<CanFail<Pong>> Handle(Ping command, CancellationToken cancellationToken)
    {
        return new Pong(command.Value + "Pong");
    }
}

public class VoidPing : ICommand
{
    public bool Called { get; set; }
}

public class VoidPingHandler : CommandHandler<VoidPing>
{
    public override Task<CanFail> Handle(VoidPing @event, CancellationToken cancellationToken)
    {
        @event.Called = true;
        return Task.FromResult(CanFail.Success);
    }
}

public class CommandTests
{
    public CommandTests()
    {
        var services = new ServiceCollection();
        services.AddCleanMediator(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
    }
    
    [Fact]
    public async Task ExecuteAsync_ShouldCallSendAsync()
    {
        //Arrange
        var ping = new VoidPing
        {
            Called = false
        };
        
        var mediator = Substitute.For<IMediator>();
        
        //Act
        _ = await mediator.ExecuteAsync(ping);
        
        //Assert
        await mediator.Received(1).SendAsync(ping);

    }

    [Fact]
    public async Task ExecuteWithResultAsync_ShouldCallSendAsync()
    {
        //Arrange
        var message = "Ping";
        var ping = new Ping(message);
        var mediator = Substitute.For<IMediator>();
        
        //Act
        _ = await mediator.ExecuteAsync(ping);
        
        //Assert
        await mediator.Received(1).SendAsync(ping);
    }
}