using IdentityServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityServer()
    .AddInMemoryApiResources(Configuration.GetApi())
    .AddInMemoryClients(Configuration.GetClients())
    .AddInMemoryApiScopes(Configuration.GetApiScopes())
    .AddDeveloperSigningCredential();

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseIdentityServer();

app.MapDefaultControllerRoute();

app.Run();
