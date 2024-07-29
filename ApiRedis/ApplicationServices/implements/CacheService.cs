using ApiRedis.ApplicationServices.Interfaces;
using ApiRedis.Configurations;

using Newtonsoft.Json;

using StackExchange.Redis;

namespace ApiRedis.ApplicationServices.implements;

public class CacheService : ICacheService
{
  private IDatabase _database;

  public CacheService()
  {
    ConfigureRedis();
  }

  private void ConfigureRedis()
  {
    _database = ConnectionHelper.Connection.GetDatabase();
  }
  
  
  public T GetData<T>(
    string key
  )
  {
    RedisValue value = _database.StringGet(key);
    if (!string.IsNullOrEmpty(value))
    {
      return JsonConvert.DeserializeObject < T > (value);
    }
    return default;
  }

  public bool SetData<T>(
    string key,
    T value,
    DateTimeOffset expirationTime
  )
  {
    TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
    bool isSet = _database.StringSet(key, JsonConvert.SerializeObject(value), expiryTime);
    return isSet;
  }

  public object RemoveData(
    string key
  )
  {
    bool isKeyExist = _database.KeyExists(key);
    return isKeyExist && _database.KeyDelete(key);
  }
}