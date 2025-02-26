using System.Collections.Concurrent;
using TinyKV.Server;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(new ConcurrentDictionary<string, string>(Environment.ProcessorCount * 2, 200_000));
builder.Services.AddSignalR(options =>
{
    options.MaximumReceiveMessageSize = 1024 * 1024; // Increase to 1MB (default is 32KB)
});

var app = builder.Build();

app.UseRouting();
app.MapHub<KVMemStore>("/tinykv");

app.Run();