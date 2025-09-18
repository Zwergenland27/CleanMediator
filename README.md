# CleanMediatR
This package will provide similar features like the (now paid) MediatR Package
and can be combined with the [CleanDomainValidation](https://github.com/Zwergenland27/CleanDomainValidation)
package to return strongly typed errors instead of exceptions.

IT also makes implementing CQRS more easy as commands, queries and events can
be seperated by default

## Defining handlers
For each of the three types a request object and a request handler must be defined.
The request object contains all necessary information as described in the
CleanDomainValidation package.
While Queries must contain a return type, Commands can be implemented without
one for write only commands. Events can never have a return type as they have
no real caller that can handle those information.

For defining request objects, simply inherit from the marker interfaces:
* `ICommand` for command without response
* `ICommand<TResult>` for commands with response of type `TResult`
* `IQuery<TResult>` for query with response of type `TResult`
* `IEvent` for an event

For handling the specified request, a command handler must be created by
inheriting from the following bases:
* `CommandHandlerBase<TCommand>` for command of type `TCommand` without a response
* `CommandHandlerBase<TCommand, TResult>` for command of type `TCommand` with response of type `TResult`
* `QueryHandlerBase<TQuery, TResult>` for query of type `TQuery` with response of type `TResult`
* `CommandHandlerBase<TEvent>` for event of type `TEvent`

Example for command with response:
```csharp
public class AddUserCommandHandler : CommandHandlerBase<AddUserCommand, AddUserResult>
{
    public Task<CanFail<AddUserResult>> Handle(AddUserCommand command, CancellationToken cancellationToken)
    {
        //command logic that may result in error
    }
}
```

Example for command without response:
```csharp
public class UserDataUpdatedEventHandler : EventHandlerBase<UserDataUpdatedEvent>
{
    public Task<CanFail> Handle(UserDataUpdatedEvent @event, CancellationToken cancellationToken)
    {
        //command logic that may result in error
    }
}
```

## Register handlers automatically
To automatically register the handlers to the Dependency Injection can be accomplished by
calling `AddCleanMediator` and defining the assemblies to register. Registering multiple assemblies
is also supported.

Example:
```csharp
builder.Services.AddCleanMediator(cfg => 
{
    cfg.RegisterServicesFromAssembly(typeof(UserDataUpdatedEventHandler).GetExecutingAssembly());
});
```

## Execute handlers
To execute the registered handlers, the `IMediator` interface can be used
with the following methods:
* `mediator.ExecuteAsync(command)` for command without response
* `mediator.ExecuteAsync<TResult>(command)` for command with response of type `TResult`
* `mediator.RunAsync<TResult>(query` for queries with response of type `TResult`
* `mediator.PublishAsync(@event)` for events

The calls will be handles asynchronously and the result is a Result object of type
`CanFail` for handlers without a result and `CanFail<TResult>` for handlers that return `TResult`.

Example:
```csharp
var command = new AddUserCommand(); //Of type ICommand<AddUserResult>
var result = await mediator.ExecuteAsync(command); //Returns CanFail<AddUserResult>
//handle result
```

## Specify generic request
For generic requests, the `IRequest` and `IRequest<TResult>` interface can be used
to mark the request object.
The handler can be implemented by creating a class inheriting from `IRequestHandler<TRequest>`
or `IRequestHandler<TRequest, TResult>` just like in the examples above.

Generic requests can be executed by calling `mediator.SendAsync(...)` or `mediator.SendAsync<TResult>(...)`.

## Sending requests without a scope
In some cases the request will be processed in the background and thus no service scope is set
even it may be required. This is especially often the case for events as they have no natural caller.
To make the life of the developer more easy, the service provider extension
`CreateAsyncMediatorScope()` can be used to create a separate service scope that can be used just
like a normal mediator. All calls made to the returned `AsyncMediatorScope` will be handled in
the same scope.

Example:
```csharp
await scope = serviceProvider.CreateAsyncMediatorScope();
//Those two handlers will be executed in the same scope
var res1 = await scope.PublishAsync(event1);
var res2 = await scope.PublishAsync(event2);

await scope = serviceProvider.CreateAsyncMediatorScope();
//This handler will be executed in a seperate scope
var res3 = await scope.PublishAsync(event3);
```