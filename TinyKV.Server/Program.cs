using TinyKV.Server;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMemoryCache();
builder.Services.AddSignalR(options =>
{
    options.MaximumReceiveMessageSize = 1024 * 1024; // Increase to 1MB (default is 32KB)
});

var app = builder.Build();

app.UseRouting();
app.MapHub<KVMemStore>("/tinykv");

app.Run();