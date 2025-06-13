using System.Reflection;
using CleanDomainValidation.Domain;
using CleanMediator;
using CleanMediator.Events;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
namespace Tests.Events;

public record VoidPing : IEvent;

public class VoidPingHandlerBase : EventHandlerBase<VoidPing>
{
    public override Task<CanFail> Handle(VoidPing query, CancellationToken cancellationToken)
    {
        return Task.FromResult(CanFail.Success);
    }
}

public class EventTests
{
    public EventTests()
    {
        var services = new ServiceCollection();
        services.AddCleanMediator(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
    }

    [Fact]
    public async Task PublishAsync_ShouldCallSendInOwnScopeAsync()
    {
        //Arrange
        var ping = new VoidPing();

        var mediator = Substitute.For<IMediator>();

        //Act
        _ = await mediator.PublishAsync(ping);

        //Assert
        await mediator.Received(1).SendAsync(ping);
    }

}