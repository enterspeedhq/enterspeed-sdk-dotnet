# Changelog

All notable changes to this project will be documented in this file.

The format is based on Keep a Changelog, and this project adheres to Semantic Versioning.

## [2.0.6 - 2026-01-21]
### Added
- Bulk ingest endpoint support for batch ingesting multiple source entities
  - `SaveBulkAsync()` method with two overloads (with and without connection parameter)
  - `BulkIngestRequest` model for individual entity data in bulk requests
  - `BulkIngestResponse` model with ChangedSourceEntities, UnchangedSourceEntities, and Errors
  - Partial success handling with individual entity validation and error reporting
  - Support for up to 50 entities per request
  - Helper properties on response: `IsFullSuccess`, `IsPartialSuccess`, `IsFullFailure`

### Fixed
- Azure Pipeline now uses ubuntu-22.04 for NuGet release to ensure Mono compatibility

### Performance
- Bulk operations provide 10-20x performance improvement over sequential single operations for batch processing
- Reduced HTTP overhead when processing multiple entities

### Changed
- Updated target frameworks to include .NET 9.0

## [2.0.4 - 2024-11-22]
### Changed
- Updated dependency on `Microsoft.Extensions.DependencyInjection.Abstractions` to version 9
- Updated dependency on `System.Text.Json` to version 9

## [2.0.3 - 2024-10-14]
### Fixed
- Fixed a potential null reference exception in `EnterspeedConnection` when not used before it's disposed again (contribution by [Marcin-Niznik](https://github.com/Marcin-Niznik))

## [2.0.2 - 2024-08-09]
### Fixed
- `parentId` on ingested source entity is only saved if `properties` is of type `IDictionary<string, IEnterspeedProperty>`

## [2.0.1 - 2023-12-11]
### Changed
- Updated dependency on `Microsoft.Extensions.DependencyInjection.Abstractions` to version 8
- Updated dependency on `System.Text.Json` to version 8

## [2.0.0 - 2023-11-14]
### Breaking
- The type for `Properties` on `IEnterspeedEntity` has changed from `IDictionary<string, IEnterspeedProperty>` to `object`

### Added
- A generic version of `IEnterspeedEntity` has been added

#### Release Notes

With this update you now have way more flexibility when building your `EnterspeedEntity`, that you need for ingest.

In V1 you had to build the properties on your `EnterspeedEntity` using `IEnterspeedProperty` 
and declare the type of each property using the different `IEnterspeedProperty` types 
(`StringEnterspeedProperty`,  `NumberEnterspeedProperty`, `BooleanEnterspeedProperty`, `ObjectEnterspeedProperty` and `ArrayEnterspeedProperty`).

**Example from Enterspeed.Source.Sdk V1**
```c#
var entity = new EnterspeedEntity
       {
           Id = "66c5174a-c173-4503-86f9-ee5bf3defe9b",
           Type = "test",
           Properties = new Dictionary<string, IEnterspeedProperty>
           {
               { "key", new StringEnterspeedProperty("test-product-name") },
               { "productType", new StringEnterspeedProperty("Accessories") },
               { "name", new ObjectEnterspeedProperty(
                     new Dictionary<string, IEnterspeedProperty>
                    {
                         { "en_US", new StringEnterspeedProperty("Test product name ") },
                         { "de_DE", new StringEnterspeedProperty("Produktbezeichnung testen") }
                    }
                ) },
            { "description", new ObjectEnterspeedProperty(
                new Dictionary<string, IEnterspeedProperty>
                 {
                     { "en_US", new StringEnterspeedProperty("Test product description") },
                     { "de_DE", new StringEnterspeedProperty("Produktbeschreibung testen1") }
                 }
            ) }
        }
    }

var response = enterspeedIngestService.Save(entity);
```

With V2 you can still do it like in V1 by using the generic version of the `EnterspeedEntity` (`EnterspeedEntity<IDictionary<string, IEnterspeedProperty>>`).

But you can do way more with V2, below are some examples of what you can do with V2.

**Ingesting raw json, e.g. data you have from a file or an API endpoint**
```c#
var entity = new EnterspeedEntity
{
    Id = "my-id",
    Type = "test",
    Properties = "{\"prop1\": \"value1\", \"prop2\": 2, \"prop3\": { \"prop4\": false } }"
};
```

**Setting properties to a custom object**
```c#
var entity = new EnterspeedEntity
{
    Id = "my-id",
    Type = "test",
    Properties = myOwnObjectType
};
```

**Using a generic version of `EnterspeedEntity`**
```c#
var entity = new EnterspeedEntity<Dictionary<string, object>>
{
    Id = "my-id",
    Type = "test",
    Properties = new Dictionary<string, object>
    {
        { "prop1", "value1" },
        { "prop2", 2 },
        { "prop3", new Dictionary<string, object> { {"prop4", false} } }
    }
};

entity.Properties.Add("prop5", "value5");
```

## [1.0.2 - 2023-04-27]
### Changed
- Reverted back to SDK v1 of the Enterspeed ingest API

## [1.0.1 - 2023-04-19]
### Fixed
- Fixed `Test` method used by connectors like (Umbraco, Sitecore, ...)

## [1.0.0 - 2023-04-18]
### Added
- Added support for `AddEnterspeedIngestService` extension method for .net 6 and greater
- Simplyfied setup by providing default value for baseUrl (https://api.enterspeed.com)

### Added
- Option to ingest a raw json string without having to map it to a entity type

### Changed
- Updated SDK to use v2 of the Enterspeed ingest API
- Updated dependencies to allow System.Text.Json v7

### Breaking
- `AddEnterspeedIngestService` moved from namespace `Enterspeed.Source.Sdk.Extensions.NETCore.Setup` to `Enterspeed.Source.Sdk.Extensions.Setup`

## [0.6.0 - 2021-11-22]

- Added extension methods for dependency injection

## [0.5.0 - 2021-11-04]

- Include Enterspeed System Header with version for this package
- Include response content and errors dictionary from Ingest service

## [0.4.1 - 2021-05-20]

- Fixed task awaiter in the EnterspeedIngestService

## [0.4.0 - 2021-03-17]

- Multi-targeted the SDK for **.NET Standard 1.1** and **.NET Standard 2.0**
- Added IJsonSerializer interface
- Added SystemTextJsonSerializer for .NET Standard 2.0
- Fixed Ingest endpoint path
  - Added version number
  - Removed /api/

## [0.3.0 - 2021-02-10]

- Added interface for EnterspeedConnection: IEnterspeedConnection.
- Added EnterspeedConfigurationProvider
- Added stylecop
- Removed EnterspeedConfigurationService

## [0.2.0 - 2021-02-09]

- Moved EnterspeedConnection.cs, IngestResponse.cs and Response.cs from **Enterspeed.Source.Sdk.Domain.Client** namespace to **Enterspeed.Source.Sdk.Domain.Connection**
- Removed MediaDomain from EnterspeedConfiguration.cs
