# __SourceName__

Roslyn source-generator package. The scaffold demonstrates an incremental generator that adds JSON serialization helpers for annotated C# classes.

```csharp
using __SourceName__;

[GenerateJsonSupport]
public sealed class Customer
{
    public string Id { get; set; } = string.Empty;
}

var json = CustomerJson.Serialize(new Customer { Id = "C-42" });
var customer = CustomerJson.Deserialize(json);
```

Replace this sample and readme with the package's real generator contract before publishing.
