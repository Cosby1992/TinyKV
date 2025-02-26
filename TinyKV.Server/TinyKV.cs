using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

namespace TinyKV.Server;
public class KVMemStore() : Hub
{
    private static readonly ConcurrentDictionary<string, string> _store = new();
    
    public Task<string?> Get(string key) => Task.FromResult(_store.TryGetValue(key, out string? value) ? value : null);
    public Task Set(string key, string value) => Task.FromResult(_ = _store[key] = value);
    public Task<bool> Has(string key) => Task.FromResult(_store.TryGetValue(key, out _));
    public Task<bool> Delete(string key) => Task.FromResult(_store.TryRemove(key, out _));

    public Task DeleteAllKeys() {
        _store.Clear();
        return Task.CompletedTask;
    }
}