# Changelog

All notable changes to this project will be documented in this file.

The format is based on Keep a Changelog, and this project adheres to Semantic Versioning.

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
