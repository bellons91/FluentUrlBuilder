# FluentUrlBuilder

Allows to create and manipulate URLs fluently.

[![Build Status](https://dev.azure.com/code4it-dev/FluentUrlBuilder/_apis/build/status/bellons91.UrlBuilder?branchName=master)](https://dev.azure.com/code4it-dev/FluentUrlBuilder/_build/latest?definitionId=1&branchName=master)

__NOTE: all the input strings are not escaped.__

## Basic example

To initialize the builder, you must provide at least one URL part.

```csharp
 var builder = UrlBuilder.Initialize("https://www.code4it.dev", "blog");
```

All the / at the beginning and at the end of every part are trimmed. So the above example is exactly the same as


```csharp
 var builder = UrlBuilder.Initialize("https://www.code4it.dev/", "/blog/");
```

To get the complete string, you must call the `GetResult` method.


```csharp
 var finalUrl = UrlBuilder.Initialize("https://www.code4it.dev/", "/blog/")
                    .GetResult();
```

Now the `finalUrl` variable contains the whole URL without additional slashes (in this case, https://www.code4it.dev/blog ).

__NOTE: null or empty strings are ignored.__


## Adding a new URL part

Even though the Initialize method accepts multiple strings (using the `params` keyword in C#)
you can add new parts dynamically by using the `AddPart` method.


