# IJsonSerializer

Abstraction to provide your own way of serializing and deserializing
within the SDK.

## .NET Standard 2.0 support

If you are using .NET Core, .NET 5, .NET Framework 4.6.1 or above
you will install the .NET Standard 2.0 version of the SDK.

This version provides an already implemented IJsonSerializer, that uses
System.Text.Json: "SystemTextJsonSerializer".

If you want to use something else, feel free to implement your own.

## .NET Standard 1.1 support

If you use anything below .NET Framework 4.6.1 you will install
the .NET Standard 1.1 version of the SDK.

This version doesn't provide any IJsonSerializer implementation, so you must implement your own.
