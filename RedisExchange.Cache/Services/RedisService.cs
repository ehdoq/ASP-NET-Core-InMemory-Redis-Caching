using StackExchange.Redis;

namespace RedisExchange.Cache.Services;

public class RedisService
{
    private readonly ConnectionMultiplexer _connectionMultiplexer;

    public RedisService(string url)
    {
        _connectionMultiplexer = ConnectionMultiplexer.Connect(url);
    }

    public IDatabase GetDb(int dbIndex)
    {
        return _connectionMultiplexer.GetDatabase(dbIndex);
    }
}