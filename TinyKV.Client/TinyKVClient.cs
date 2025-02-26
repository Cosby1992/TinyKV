using Microsoft.AspNetCore.SignalR.Client;
namespace TinyKV.Client;

public class TinyKVClient(string serverUrl)
{
    private readonly HubConnection _connection = new HubConnectionBuilder()
            .WithUrl(serverUrl)
            .WithAutomaticReconnect()
            .Build();

    public async Task StartAsync() => await _connection.StartAsync();

    // Single Operations
    public async Task SetAsync(string key, string value) => await _connection.InvokeAsync("Set", key, value);
    public async Task<string> GetAsync(string key) => await _connection.InvokeAsync<string>("Get", key);
    public async Task<bool> HasAsync(string key) => await _connection.InvokeAsync<bool>("Has", key);
    public async Task<bool> DeleteAsync(string key) => await _connection.InvokeAsync<bool>("Delete", key);
    public async Task ClearAsync(string key) => await _connection.InvokeAsync<bool>("Clear", key);

    // Batch Operations
    public async Task SetBatchAsync(Dictionary<string, string> keyValues) =>
        await _connection.InvokeAsync("SetBatch", keyValues);

    public async Task<Dictionary<string, string>> GetBatchAsync(List<string> keys) =>
        await _connection.InvokeAsync<Dictionary<string, string>>("GetBatch", keys);

    public async Task<bool[]> HasBatchAsync(List<string> keys) =>
        await _connection.InvokeAsync<bool[]>("HasBatch", keys);

    public async Task<int> DeleteBatchAsync(List<string> keys) =>
        await _connection.InvokeAsync<int>("DeleteBatch", keys);
}
