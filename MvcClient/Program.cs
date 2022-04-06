var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "cookie";
    options.DefaultAuthenticateScheme = "oidc";
})
    .AddCookie("cookie")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://localhost:7104";
        options.ClientId = "client_id_mvc";
        options.ClientSecret = "client_secret_mvc";
        options.SaveTokens = true;
       
        options.ResponseType = "code";
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization(); 

app.MapDefaultControllerRoute();

app.Run();
