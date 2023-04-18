# [Enterspeed .NET SDK](https://www.enterspeed.com/) &middot; [![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](./LICENSE) [![NuGet version](https://img.shields.io/nuget/v/Enterspeed.Source.Sdk)](https://www.nuget.org/packages/Enterspeed.Source.Sdk/) [![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)](https://github.com/enterspeedhq/enterspeed-sdk-dotnet/pulls) [![Twitter](https://img.shields.io/twitter/follow/enterspeedhq?style=social)](https://twitter.com/enterspeedhq)

## Version 

A list of Enterspeed endpoint versions that the SDKs uses.

### Enterspeed.Source.Sdk

The package uses __v2__ of the Enterspeed Ingest API version

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

Create entity classes for your models by implementing the `IEnterspeedEntity` interface or inheriting from `EnterspeedEntity` class.

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
