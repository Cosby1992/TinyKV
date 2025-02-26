using TinyKV.Server;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMemoryCache();
builder.Services.AddSignalR();

var app = builder.Build();

app.UseRouting();
app.MapHub<KVMemStore>("/tinykv");

app.Run();