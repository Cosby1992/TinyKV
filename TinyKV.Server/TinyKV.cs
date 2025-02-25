using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;

namespace TinyKV.Server;
public class Store(IMemoryCache cache) : Hub
{
    private readonly IMemoryCache _cache = cache;

    public Task<string> Get(string key)
    {
        _cache.TryGetValue(key, out string? value);
        return Task.FromResult(value ?? "");
    }

    public async Task Set(string key, string value)
    {
        _cache.Set(key, value);
        await Clients.All.SendAsync("OnValueChanged", key, value);
    }

    public async Task<bool> Has(string key)
    {
        await Task.CompletedTask;
        return _cache.TryGetValue(key, out _);
    }

    public Task<bool> Delete(string key)
    {
        _cache.Remove(key);
        return Task.FromResult(true);
    }
}