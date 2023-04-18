# Changelog

All notable changes to this project will be documented in this file.

The format is based on Keep a Changelog, and this project adheres to Semantic Versioning.

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
