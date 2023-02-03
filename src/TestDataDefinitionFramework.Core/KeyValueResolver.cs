using System;
using System.Collections.Generic;

namespace TestDataDefinitionFramework.Core;

/// <summary>
/// Use this class to define functions that are used when inserting data into Key Value stores, such as Redis.
/// </summary>
public class KeyValueResolver
{
    private readonly Dictionary<Type, object> _resolvers = new();

    public (string Key, string Value) GetKeyValue<T>(T item)
    {
        if (!_resolvers.TryGetValue(typeof(T), out var resolver))
            throw new NotSupportedException($"No key value resolver was registered for type {typeof(T).FullName}");
        if (resolver is not Func<T, (string, string)> typedResolver) 
            throw new NotSupportedException($"Key value resolver for {typeof(T).FullName} was not of the expected type.");

        return typedResolver(item);
    }

    public KeyValueResolver WithResolver<T>(Func<T, (string, string)> keyValueResolver)
    {
        _resolvers.Add(typeof(T), keyValueResolver);
        return this;
    }
}