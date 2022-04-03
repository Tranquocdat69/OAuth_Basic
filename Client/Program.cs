using Microsoft.AspNetCore.Authentication.OAuth;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "ClientCookie";
    options.DefaultSignInScheme = "ClientCookie";
    options.DefaultChallengeScheme = "OurServer";
})
    .AddCookie("ClientCookie")
    .AddOAuth("OurServer", options =>
    {
        options.AuthorizationEndpoint = "https://localhost:7151/oauth/authorize";
        //"https://github.com/login/oauth/authorize"; 
        options.ClientId = "client_id";
        //"Iv1.ba390723dc2333f2";
        options.ClientSecret = "client_secret";
        //"20f2225b0387d8f750c199ccf06b7eccdcab8caf";
        options.CallbackPath = "/callback";
        options.TokenEndpoint = "https://localhost:7151/oauth/token";
        //"https://github.com/login/oauth/access_token"; 

        /*options.Scope.Add("secret");
        options.Scope.Add("library");
        options.Scope.Add("bank");*/

        options.SaveTokens = true;

        options.Events = new OAuthEvents()
        {
            OnCreatingTicket = context =>
            {
                var accessToken = context.AccessToken;
                string base64Payload = accessToken.Split('.')[1];
                // at least 2 char (base 64) to represent 1 char (base 256) 
                switch (base64Payload.Length % 4)
                {
                    case 0:
                        break;
                    case 2:
                        base64Payload += "==";
                        break;
                    case 3:
                        base64Payload += "=";
                        break;
                    default:
                        throw new Exception("Illegal string base64");
                }
                byte[] bytePayload = Convert.FromBase64String(base64Payload);
                string jsonPayload = Encoding.UTF8.GetString(bytePayload, 0, bytePayload.Length);
                Dictionary<string, string> dictionaryPayload = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonPayload);

                foreach (var item in dictionaryPayload)
                {
                    context.Identity.AddClaim(new Claim(item.Key, item.Value));
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddHttpClient();
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
