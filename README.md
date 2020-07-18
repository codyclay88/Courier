# Courier

### *This document is in progress!*

A simple messaging library for .NET applications. 

Courier provides a single abstraction that enables developers to build more reactive, event-driven application components - whether those components are running in the same process, across the network, or running in the browser (via WebAssembly/Blazor). 

When it comes to messaging infrastructure, you can imagine that your application code could look significantly different if your application components are all running in the same process as opposed to being distributed across the network. 

The goal is that when developers transition from one architectural style to another (or even between environments) that their application code will not change - only configuration. 



## Dispatching and Subscribing

Courier does this by exposing two methods: `Dispatch` and `Subscribe` (each method has a ton of overloads).

When something notable happens in your application, you can let other components of your app know about it by "dispatching" an event describing what happened. 

```csharp
courier.Dispatch(new SomethingAwesomeHappened("Billy Madison is having a sequel."));
```

The `Dispatch` methods accepts an instance of a class that implements `ICourierEvent`, 

To be informed of this event in another part of your application, you need to subscribe to it. You can subscribe to the event in a couple different ways:

1. By providing an `Action<TEvent>`

```csharp
courier.Subscribe<SomethingAwesomeHappened>(e => Console.WriteLine("This is the best news ever!"));
```

2. By defining a class that implements `ICourierListener<TEvent>`, and providing an instance to `Subscribe` or defining a factory method:
```csharp
public class LoverOfAwesomeThings : ICourierListener<SomethingAwesomeHappened>
{
    private readonly ILogger<LoverOfAwesomeThings> _logger;

    public LoverOfAwesomeThings(ILogger<LoggerOfAwesomeThings> logger)
    {
        _logger = logger;
    }

    public Task Process(SomethingAwesomeHappened @event)
    {
        _logger_.LogInformation("This is the best news ever!")
        return Task.CompletedTask;
    }
}

// Provide an instance of `LoverOfAwesomeThings` that will handle all occurrences. 
courier.Subscribe<SomethingAwesomeHappened>(new LoverOfAwesomeThings(logger));

// Or provide a factory method that will create an instance for each occurrence
courier.Subscribe<SomethingAwesomeHappened>(() => _container.Resolve<LoverOfAwesomeThings>());
```

Under the hood, Courier stands on the shoulders of Reactive Extensions. If you need more control over your event subscriptions then you can subscribe directly to the event stream:

```csharp
var sub = courier.Events.Subscribe((e) => 
{
    switch (e)
    {
        case SomethingAwesomeHappened _:
            Console.WriteLine("This is the best news ever!");
            break;
        default:
            break;
    }
});

// Later on, dispose the subscription
sub.Dispose();
```


## Concepts

An instance of an `ICourier` acts as a local message broker between your application components. Each executable has only a single `ICourier` instance, meaning that if all your components are running in-process (in a more traditional "monolithic" architecture, not to say that this )

## Getting Started 

Install the `Clayware.Courier` NuGet package (does not exist yet, he-he). 

`$ dotnet add package Clayware.Courier`

For ASP.NET Core applications, 


## Roadmap

**TODO**