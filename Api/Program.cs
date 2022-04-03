using Api.AuthorizationRequirement;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization(options =>
{
    var policyBuilder = new AuthorizationPolicyBuilder();
    AuthorizationPolicy authorizationPolicy = policyBuilder
    .AddRequirements(new JwtRequirement())
    .Build();

    options.DefaultPolicy = authorizationPolicy;
});
builder.Services
    .AddHttpClient()
    .AddHttpContextAccessor();

builder.Services.AddScoped<IAuthorizationHandler, JwtRequirementHandler>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();
