using Microsoft.AspNetCore.SignalR.Client;
namespace TinyKV.Client;

public class TinyKVClient(string serverUrl)
{
    private readonly HubConnection _connection = new HubConnectionBuilder()
            .WithUrl(serverUrl)
            .WithAutomaticReconnect()
            .Build();

    public async Task StartAsync() => await _connection.StartAsync();
    public async Task SetAsync(string key, string value) => await _connection.InvokeAsync("Set", key, value);
    public async Task<string> GetAsync(string key) => await _connection.InvokeAsync<string>("Get", key);
    public async Task<bool> DeleteAsync(string key) => await _connection.InvokeAsync<bool>("Delete", key);
}
