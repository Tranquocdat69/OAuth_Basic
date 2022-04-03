using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Server;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("OAuth")
    .AddJwtBearer("OAuth", options =>
    {
        var secretKeyByte = Encoding.UTF8.GetBytes(ConfigJwt.SecretKey);
        var symetricKey = new SymmetricSecurityKey(secretKeyByte);

        // Headers Authorization: Beaer 123abc
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidIssuer = ConfigJwt.Issuer,
            ValidAudience = ConfigJwt.Audience,
            IssuerSigningKey = symetricKey,
            ClockSkew = TimeSpan.Zero
        };

        //url?access_token=123abc
        options.Events = new JwtBearerEvents()
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Query.ContainsKey("access_token"))
                {
                    context.Token = context.Request.Query["access_token"];
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();
