# IEnterspeedProperty

IEnterspeed property is the interface used for all types
of properties that Enterspeed can handle.  

## Types of properties

There are 5 different types of properties, as listed here:

____

### ArrayEnterspeedProperty : IEnterspeedProperty

|Name    | Type                  |Description |
|:----   |:-----                 |:-----      |
|Name    |`string`               | Name of the property
|Type    |`string`               | "array"
|Items   |`IEnterspeedProperty[]`| Array of items

____

### BooleanEnterspeedProperty : IEnterspeedProperty

|Name    |Type       |Description |
|:----   |:-----     |:-----      |
|Name    |`string`   |Name of the property
|Type    |`string`   |"boolean"
|Value   |`bool`     |Boolean value

____

### NumberEnterspeedProperty : IEnterspeedProperty

|Name     |Type      |Description |
|:----    |:-----    |:-----      |
|Name     |`string`  |Name of the property
|Type     |`string`  |"number"
|Value    |`double`  |Double value
|Precision|`int`     |Default 0

____

### ObjectEnterspeedProperty : IEnterspeedProperty

|Name       |Type                                       |Description |
|:----      |:-----                                     |:-----|
|Name       |`string`                                   |Name of the property
|Type       |`string`                                   |"object"
|Properties |`Dictionary<string, IEnterspeedProperty>`  |Properties of the object

____

### StringEnterspeedProperty : IEnterspeedProperty

|Name       |Type         |Description |
|:----      |:-----       |:-----      |
|Name       |`string`     | Name of the property
|Type       |`string`     | "string"
|Value      |`string`     | String value

____
