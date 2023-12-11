# Entities

## IEnterspeedEntity

An IEnterspeedEntity is the model,
that is being sent to the Enterspeed Ingest API,  
for creating/updating data.

|Name               | Type                                      |Description |
|:----              | :-----                                    |:-----|
|Id                 | `string`                                  | Unique identifier
|Type               | `string`                                  | Type of the data
|Url                | `string`                                  | The absolute url of the data
|Redirects          | `string[]`                                | Array of redirects for the data
|ParentId           | `string`                                  | Unique identifier of the parent
|Properties         | `object`                                  | Custom properties

## IEnterspeedEntity\<T>

An IEnterspeedEntity is the model,
that is being sent to the Enterspeed Ingest API,  
for creating/updating data.

|Name               | Type                                      |Description |
|:----              | :-----                                    |:-----|
|Id                 | `string`                                  | Unique identifier
|Type               | `string`                                  | Type of the data
|Url                | `string`                                  | The absolute url of the data
|Redirects          | `string[]`                                | Array of redirects for the data
|ParentId           | `string`                                  | Unique identifier of the parent
|Properties         | `T`                                       | Custom properties

## [IEnterspeedProperty implementations](./properties/README.md)

List of implementations that the SDK ships with.
