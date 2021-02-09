# IEnterspeedConfigurationService

Interface for saving and getting the configuration for Enterspeed.

The Enterspeed Configuration Service is used to serve
the [EnterspeedConfiguration](./../../configuration/README.md) to the [EnterspeedConnection](./../../connection/README.md).

## Default implementation of IEnterspeedConfigurationService

The SDK ships with an `InMemoryEnterspeedConfigurationService` implementation,  
that can be used to serve the EnterspeedConfiguration.

If needed you can implement your own IEnterspeedConfigurationService  
if you need to fetch the configuration from a database or similar.
