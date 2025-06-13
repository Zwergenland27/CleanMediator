using System.Reflection;
using CleanDomainValidation.Domain;
using CleanMediator;
using CleanMediator.Queries;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Tests.Queries;

public record Pong;

public record Ping : IQuery<Pong>;

public class PingHandlerBase : QueryHandlerBase<Ping, Pong>
{
    public override Task<CanFail<Pong>> Handle(Ping query, CancellationToken cancellationToken)
    {
        var result = new CanFail<Pong>();
        result.Succeeded(new Pong());
        return Task.FromResult(result);
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
        var ping = new Ping();
        var mediator = Substitute.For<IMediator>();
        
        //Act
        _ = await mediator.RunAsync(ping);
        
        //Assert
        await mediator.Received(1).SendAsync(ping);
    }
}