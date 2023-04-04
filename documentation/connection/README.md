# Enterspeed Connection

The Enterspeed Connection simply establishes and maintains
a connection to the configured Enterspeed endpoint.

For the connection to work, 1 configuration setting in the
[EnterspeedConfiguration](./../configuration/README.md) _must_ be set:

* ApiKey
* BaseUrl

## Base URL

The `BaseUrl` has a default value of https://api.enterspeed.com, but can be overridden on the `EnterspeedConfiguration`.

## Connection timeout

The connection is reused for the time set in the configuration **ConnectionTimeout**.  
When the connection hits the timeout, it will automatically create a new connection.

## Connection flush

If the connection needs to be renewed programatically  
the `Flush()` method can be called and the connection will be reset.
