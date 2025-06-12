using System.Reflection;
using CleanDomainValidation.Domain;
using CleanMediator;
using CleanMediator.Commands;
using CleanMediator.Queries;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Tests.Queries;

public record Pong(string Value);

public record Ping(string Value) : IQuery<Pong>;

public class PingHandler : QueryHandler<Ping, Pong>
{
    public override async Task<CanFail<Pong>> Handle(Ping request, CancellationToken cancellationToken)
    {
        return new Pong(request.Value + "Pong");
    }
}

public class QueryTests
{
    public QueryTests()
    {
        var services = new ServiceCollection();
        services.AddCleanMediator(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
    }

    [Fact]
    public async Task ExecuteWithResultAsync_ShouldCallSendAsync()
    {
        //Arrange
        var message = "Ping";
        var ping = new Ping(message);
        var mediator = Substitute.For<IMediator>();
        
        //Act
        _ = await mediator.RunAsync(ping);
        
        //Assert
        await mediator.Received(1).SendAsync(ping);
    }
}