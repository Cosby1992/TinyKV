using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;

namespace TinyKV.Server;
public class Store(IMemoryCache cache) : Hub
{
    public string? Get(string key)
    { 
        cache.TryGetValue(key, out string? value);
        return value;
    }

    public void Set(string key, string value)
    {
        _ = cache.Set(key, value);
    }

    public bool Has(string key)
    {
        return cache.TryGetValue(key, out _);
    }

    public bool Delete(string key)
    {
        cache.Remove(key);
        return true;
    }
}