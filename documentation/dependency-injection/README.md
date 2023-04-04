# Dependency Injection

## UseEnterspeedIngestService

An extension method to easily add the Enterspeed Ingest Service for dependency injection.

Example:

```csharp
using Enterspeed.Source.Sdk.Extensions.Setup;

serviceCollection.AddEnterspeedIngestService(new EnterspeedConfiguration
{
    ApiKey = "source-xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"
});
```

See [Enterspeed Configuration](../configuration/README.md) for configuration details.
