# [Enterspeed .NET SDK](https://www.enterspeed.com/) &middot; [![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](./LICENSE) [![NuGet version](https://img.shields.io/nuget/v/Enterspeed.Source.Sdk)](https://www.nuget.org/packages/Enterspeed.Source.Sdk/) [![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)](https://github.com/enterspeedhq/enterspeed-sdk-dotnet/pulls) [![Twitter](https://img.shields.io/twitter/follow/enterspeedhq?style=social)](https://twitter.com/enterspeedhq)

## Installation

With .NET CLI

```bash
dotnet add package Enterspeed.Source.Sdk --version <version>
```

With Package Manager

```bash
Install-Package Enterspeed.Source.Sdk -Version <version>
```

## Getting started

**1) Register services**

Register required services by calling the `AddEnterspeedIngestService` extension method and provide your API key.

````csharp
using Enterspeed.Source.Sdk.Extensions.Setup;

serviceCollection.AddEnterspeedIngestService(new EnterspeedConfiguration
{
    ApiKey = "source-xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"
});
````

**2) Create entity models**

Create entity classes for your models by implementing the `IEnterspeedEntity` interface or inheriting from `EnterspeedEntity` class
or simply just use the `EnterspeedEntity` class directly.

````csharp
using Enterspeed.Source.Sdk.Api.Models;

public class ProductEntity : EnterspeedEntity
{
    public ProductEntity(string id)
    {
        Id = id;
        Type = "product";
    }
}
````

**3) Ingest data**

Use the `IEnterspeedIngestService` to ingest your entities into Enterspeed.

````csharp
using Enterspeed.Source.Sdk.Api.Models;
using Enterspeed.Source.Sdk.Api.Services;

public class IngestService
{
    private readonly IEnterspeedIngestService _enterspeedIngestService;

    public IngestService(IEnterspeedIngestService enterspeedIngestService)
    {
        _enterspeedIngestService = enterspeedIngestService;
    }

    public void Ingest(IEnterspeedEntity enterspeedEntity)
    {
        _enterspeedIngestService.Save(enterspeedEntity);
    }
}
````

## Bulk Operations

For improved performance when ingesting or deleting multiple entities, use the bulk operation methods:

### Bulk Ingest

````csharp
using Enterspeed.Source.Sdk.Api.Models.Bulk;

var entities = new List<BulkIngestEntity>
{
    new BulkIngestEntity
    {
        OriginId = "product-123",
        Type = "product",
        Url = "/products/product-123",
        Properties = new Dictionary<string, object>
        {
            ["name"] = "Example Product",
            ["price"] = 99.99,
            ["inStock"] = true
        }
    },
    new BulkIngestEntity
    {
        OriginId = "product-124",
        Type = "product",
        Properties = new Dictionary<string, object>
        {
            ["name"] = "Another Product",
            ["price"] = 149.99
        }
    }
};

var response = _enterspeedIngestService.SaveBulk(entities);

if (response.IsFullSuccess)
{
    Console.WriteLine($"Successfully ingested {response.SuccessCount} entities");
    Console.WriteLine($"Changed: {response.ChangedSourceEntities.Count}, Unchanged: {response.UnchangedSourceEntities.Count}");
}
else if (response.IsPartialSuccess)
{
    Console.WriteLine($"Partial success: {response.SuccessCount} succeeded, {response.ErrorCount} failed");
    foreach (var failedId in response.FailedOriginIds)
    {
        var errors = response.GetErrorsForOriginId(failedId);
        Console.WriteLine($"{failedId}: {string.Join(", ", errors)}");
    }
}
````

### Bulk Delete

````csharp
var originIds = new[] { "product-123", "product-124", "product-125" };
var response = _enterspeedIngestService.DeleteBulk(originIds);

Console.WriteLine($"Deleted: {response.DeletedSourceEntities.Count}");
Console.WriteLine($"Not found: {response.NotFoundSourceEntities.Count}");
Console.WriteLine($"Errors: {response.ErrorCount}");
````

**Important Notes:**
- Bulk operations require the `IsAsyncBulkProcessingEnabled` feature flag on your tenant
- Maximum 50 entities per request (configurable per tenant)
- Maximum request size: 210MB (configurable per tenant)
- Partial success is supported - some entities may succeed while others fail
- Bulk operations provide 10-20x performance improvement over sequential single operations

## Documentation

If you need more in depth details, please look through our documentation:  
[Enterspeed Source SDK documentation](https://github.com/enterspeedhq/enterspeed-sdk-dotnet/blob/develop/documentation/README.md)

## Changelog

See new features, fixes and breaking changes in the [Changelog](https://github.com/enterspeedhq/enterspeed-sdk-dotnet/blob/develop/CHANGELOG.md).

## Contributing

Pull requests are very welcome.  
Please fork this repository and make a PR when you are ready.  

Otherwise you are welcome to open an Issue in our [issue tracker](https://github.com/enterspeedhq/enterspeed-sdk-dotnet/issues).

## License

Enterspeed .NET SDK is [MIT licensed](https://github.com/enterspeedhq/enterspeed-sdk-dotnet/blob/develop/LICENSE)
