using System.Collections.Concurrent;
using Microsoft.Extensions.Options;

namespace TinyKV.Server;

public class Program
{
    public static WebApplication CreateApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<SignalRConfig>(builder.Configuration.GetSection("SignalR"));
        builder.Services.Configure<StoreConfig>(builder.Configuration.GetSection("Store"));

        builder.Services.AddSingleton(serviceProvider =>
        {
            var storeConfig = serviceProvider.GetRequiredService<IOptions<StoreConfig>>().Value;
            return new ConcurrentDictionary<string, string>(storeConfig.ConcurrencyLevel, storeConfig.InitialCapacity);
        });

        builder.Services.AddSignalR();

        var app = builder.Build();
        app.UseRouting();
        app.MapHub<KVMemStore>("/tinykv");

        return app;
    }

    public static void Main(string[] args)
    {
        CreateApp(args).Run();
    }
}