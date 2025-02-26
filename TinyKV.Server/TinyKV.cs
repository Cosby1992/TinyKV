using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

namespace TinyKV.Server;
public class KVMemStore(ConcurrentDictionary<string, string> store) : Hub
{
    private readonly ConcurrentDictionary<string, string> _store = store;

    public Task<string?> Get(string key) => Task.FromResult(_store.TryGetValue(key, out string? value) ? value : null);
    public Task Set(string key, string value) => Task.FromResult(_ = _store[key] = value);
    public Task<bool> Has(string key) => Task.FromResult(_store.TryGetValue(key, out _));
    public Task<bool> Delete(string key) => Task.FromResult(_store.TryRemove(key, out _));

    public Task Clear()
    {
        _store.Clear();
        return Task.CompletedTask;
    }

    // Batch functions
    public Task SetBatch(Dictionary<string, string> keyValues)
    {
        foreach (var kv in keyValues)
        {
            _store[kv.Key] = kv.Value;
        }
        return Task.CompletedTask;
    }

    public Task<Dictionary<string, string>> GetBatch(List<string> keys)
    {
        var results = new Dictionary<string, string>();
        foreach (var key in keys)
        {
            if (_store.TryGetValue(key, out var value))
            {
                results[key] = value;
            }
        }
        return Task.FromResult(results);
    }

    public Task<bool[]> HasBatch(List<string> keys)
    {
        var results = keys.Select(key => _store.ContainsKey(key)).ToArray();
        return Task.FromResult(results);
    }

    public Task<int> DeleteBatch(List<string> keys)
    {
        int deletedCount = 0;
        foreach (var key in keys)
        {
            if (_store.TryRemove(key, out _))
                deletedCount++;
        }
        return Task.FromResult(deletedCount);
    }
}