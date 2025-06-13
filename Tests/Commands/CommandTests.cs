using System.Reflection;
using CleanDomainValidation.Domain;
using CleanMediator;
using CleanMediator.Commands;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using ICommand = CleanMediator.Commands.ICommand;

namespace Tests.Commands;

public record Pong;

public record Ping : ICommand<Pong>;

public class PingHandler : ICommandHandler<Ping, Pong>
{
    public Task<CanFail<Pong>> Handle(Ping query, CancellationToken cancellationToken)
    {
        var result = new CanFail<Pong>();
        result.Succeeded(new Pong());
        return Task.FromResult(result);
    }
}

public record VoidPing : ICommand;

public class VoidPingHandlerBase : CommandHandlerBase<VoidPing>
{
    public override Task<CanFail> Handle(VoidPing query, CancellationToken cancellationToken)
    {
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
        var ping = new VoidPing();
        
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
        var ping = new Ping();
        var mediator = Substitute.For<IMediator>();
        
        //Act
        _ = await mediator.ExecuteAsync(ping);
        
        //Assert
        await mediator.Received(1).SendAsync(ping);
    }
}