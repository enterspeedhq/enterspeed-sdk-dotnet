# Dependency Injection

## UseEnterspeedIngestService

An extension method to easily add the Enterspeed Ingest Service for dependency injection.

Example:

```csharp
using Enterspeed.Source.Sdk.Extensions.NETCore.Setup;

serviceCollection.AddEnterspeedIngestService(new EnterspeedConfiguration
{
    ApiKey = "source-xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
    BaseUrl = "https://api.enterspeed.com"
});
```

See [Enterspeed Configuration](../configuration/README.md) for configuration details.
