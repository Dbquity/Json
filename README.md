# Lightweight C# Json parser and programming model
If your task is to parse json whose structure is known ahead of time, a few helper functions may do the trick. For instance

    static string QuickAndDirtyJsonLookupText(string json, string key) =>
        QuickAndDirtyJsonFind(json, key).Split('"')[1];
    static string QuickAndDirtyJsonFind(string json, string key) {
        int keyIndex = json.IndexOf($"\"{key}\":");
        if (keyIndex < 0)
            throw new KeyNotFoundException($"Key \"{key}\" not found in '{json}'.");
        return json.Substring(keyIndex + 3 + key.Length).TrimStart();
    }

lets you find objects within objects and look up simple text values.

TODO: example

However, you'll often want to approach problems in a manner that is a little bit more structured, and that is what this project tries to do.
* It includes a C# object model for representing json, in which you find `Object`, `Array`, `Text`, `Number`, `Bool`, `Null` types that all inherit from `JsonValue`.
* It supports parsing a string into this representation as well as `ToString`ing to a dense textual representation and `Format`ting with line breaks and indentation.
* Finally it allows for conversion to and from user defined C# objects - subject to certain restrictions.

TODO: examples

This json implementation is used when handling RESTful interaction with internet based storage services in the [Dbquity](http://Dbquity.com) platform.

The whole thing is kept as minimal as possible to the author who would very much welcome any input :-)
