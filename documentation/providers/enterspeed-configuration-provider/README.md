# Enterspeed Configuration Provider

The `IEnterspeedConfigurationProvider` is used to serve  
the [Enterspeed Configuration](./../../configuration/README.md) to the [Enterspeed Connection](./../../connection/README.md).

Before you can use the Enterspeed Connection you must implement  
you own `IEnterspeedConfigurationProvider`.

A simple implementation could just be something like this:

```csharp
public class InMemoryEnterspeedConfigurationProvider : IEnterspeedConfigurationProvider
{
    public InMemoryEnterspeedoConfigurationProvider(EnterspeedConfiguration configuration)
    {
        Configuration = configuration;
    }

    public EnterspeedConfiguration Configuration { get; }
}
```
