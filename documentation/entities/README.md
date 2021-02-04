# Entities

## IEnterspeedEntity

An IEnterspeedEntity is the converted model,
that is being sent to the Enterspeed Ingest API,  
for creating/updating content.

|Name               | Type                                      |Description |
|:----              | :-----                                    |:-----|
|Id                 | `string`                                  | Unique identifier
|Type               | `string`                                  | Type of the data
|Url                | `string`                                  | The absolute url of the data
|Redirects          | `string[]`                                | Array of redirects for the data
|ParentId           | `string`                                  | Unique identifier of parent
|Properties         | `Dictionary<string, IEnterspeedProperty>` | Dictionary of properties

## [IEnterspeedProperty implementations](./properties/README.md)

List of implementations that the SDK ships with.
