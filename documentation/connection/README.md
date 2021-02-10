# Enterspeed Connection

The Enterspeed Connection simply establishes and maintains
a connection to the configured Enterspeed endpoint.

For the connection to work, 2 configuration settings in the
[EnterspeedConfiguration](./../configuration/README.md) _must_ be set:

* ApiKey
* BaseUrl

## Connection timeout

The connection is reused for the time set in the configuration **ConnectionTimeout**.  
When the connection hits the timeout, it will automatically create a new connection.

## Connection flush

If the connection needs to be renewed programatically  
the `Flush()` method can be called and the connection will be reset.
