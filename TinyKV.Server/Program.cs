using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using TinyKV.Server;

var builder = WebApplication.CreateBuilder(args);

// Bind configuration sections to classes
builder.Services.Configure<SignalRConfig>(builder.Configuration.GetSection("SignalR"));
builder.Services.Configure<StoreConfig>(builder.Configuration.GetSection("Store"));

builder.Services.AddSingleton(serviceProvider =>
{
    var storeConfig = serviceProvider.GetRequiredService<IOptions<StoreConfig>>().Value;
    return new ConcurrentDictionary<string, string>(storeConfig.ConcurrencyLevel, storeConfig.InitialCapacity);
});

builder.Services.AddSignalR(options =>
{
    var signalRConfig = builder.Configuration.GetSection("SignalR").Get<SignalRConfig>() ?? new SignalRConfig();
    options.MaximumReceiveMessageSize = signalRConfig.MaximumReceiveMessageSize;
});

var app = builder.Build();

app.UseRouting();
app.MapHub<KVMemStore>("/tinykv");

app.Run();