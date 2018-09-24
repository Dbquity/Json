# Lightweight C# Json parser and programming model
If your task is to parse json whose structure is known ahead of time, a few "quick and dirty" helper functions may do the trick. For instance

```csharp
static string JsonLookupText(string json, string key) =>
    JsonFind(json, key).Split('"')[1];
static string JsonFind(string json, string key) {
    int keyIndex = json.IndexOf($"\"{key}\":");
    if (keyIndex < 0)
        throw new KeyNotFoundException($"Key \"{key}\" not found in '{json}'.");
    return json.Substring(keyIndex + 3 + key.Length).TrimStart();
}
```

lets you find objects within objects and look up simple text values.

Given

```json
{
    "name": "Dbquity",
    "established": 2016,
    "primes": [ 1, 2, 3, 5, 7, 11 ],
    "engineer": { "name": "Lars", "homeTown": "Frederiksberg" }
}
```

you can

```csharp
const string json = "{ \"name\": \"Dbquity\", <... as above ...> }"; 
Assert.AreEqual("Dbquity", JsonLookupText(json, "name"));
string engineer = JsonFind(json, "engineer");
Assert.AreEqual("Lars", JsonLookupText(engineer, "name"));
Assert.AreEqual("Frederiksberg", JsonLookupText(json, "homeTown"));
```

However, you'll often want to approach problems in a manner that is a little bit more structured, and that is what this project tries to do.
* It includes a C# object model for representing json, in which you find `JsonObject`, `JsonArray`, `JsonText`, `JsonNumber`, `JsonBool`, `JsonNull` types that all inherit from `JsonValue`.
* It supports parsing a string into this representation as well as `ToString`ing to a dense textual representation and `Format`ting with line breaks and indentation.
* Finally it allows for conversion to and from user defined C# objects - subject to certain restrictions.

Reusing the example json from above, the library lets you write

```csharp
JsonObject dbquity = JsonObject.Parse(json);
Assert.AreEqual<string>("Dbquity", dbquity["name"]);
Assert.AreEqual<string>("Lars", dbquity["engineer"]["name"]);
Assert.AreEqual<int>(7, dbquity["primes"][4]);
Assert.AreEqual(6, dbquity["primes"].Array.Count);
Clipboard.SetData(DataFormats.Text, dbquity.Format());
```

Where the last line formats the json onto the clipboard like this:

```json
{
    "name": "Dbquity",
    "established": 2016,
    "primes": [
        1,
        2,
        3,
        5,
        7,
        11
    ],
    "engineer": {
        "name": "Lars",
        "homeTown": "Frederiksberg"
    }
}
```

This json implementation is used when handling RESTful interaction with internet based storage services in the [Dbquity](http://Dbquity.com) platform.

The whole thing is kept as minimal as possible to the author who would very much welcome any input :-)
