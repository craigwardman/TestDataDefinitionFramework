namespace TestDataDefinitionFramework.Testing.ExampleSut.Redis;

public interface IRedisSerializer
{
    string Serialize<T>(T item);

    T Deserialize<T>(string value);
}