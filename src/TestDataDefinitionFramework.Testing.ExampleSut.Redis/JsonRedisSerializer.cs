using Newtonsoft.Json;

namespace TestDataDefinitionFramework.Testing.ExampleSut.Redis;

public class JsonRedisSerializer : IRedisSerializer
{
    public string Serialize<T>(T item)
    {
        return JsonConvert.SerializeObject(item);
    }

    public T? Deserialize<T>(string? value)
    {
        return JsonConvert.DeserializeObject<T>(value ?? string.Empty);
    }
}