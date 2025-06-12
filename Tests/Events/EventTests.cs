using System.Reflection;
using CleanDomainValidation.Domain;
using CleanMediator;
using CleanMediator.Events;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
namespace Tests.Events;

public class VoidPing : IEvent
{
    public bool Called { get; set; }
}

public class VoidPingHandler : IEventHandler<VoidPing>
{
    public Task<CanFail> Handle(VoidPing @event, CancellationToken cancellationToken)
    {
        @event.Called = true;
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
        var ping = new VoidPing
        {
            Called = false
        };

        var mediator = Substitute.For<IMediator>();

        //Act
        _ = await mediator.PublishAsync(ping);

        //Assert
        await mediator.Received(1).SendInOwnScopeAsync(ping);
    }

}