using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Music.API.Interfaces;
using MusicStream.Infrastructure.Auth;

namespace Music.API.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApiLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuth(configuration);
    }

    public static void AddEndpoints(this IEndpointRouteBuilder app)
    {
        var iEndpoint = typeof(IEndpoint);
        var endpoints = Assembly.GetExecutingAssembly()
        .GetTypes()
        .Where(t => t is { IsClass: true, IsAbstract: false } & iEndpoint.IsAssignableFrom(t));
        foreach (var item in endpoints)
        {
            var instance = Activator.CreateInstance(item) as IEndpoint;
            var route = instance?.Register(app);
        }

    }

    public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(option =>
        {
            option.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {

                    var token = context.HttpContext.Request.Cookies["access_token"];
                    context.Token = token;
                    return Task.CompletedTask;
                }
            };

            var jwtOption = configuration.GetSection("JwtSetting").Get<JwtOption>()
             ?? throw new Exception("Something is wrong with Jwt token setting");


            option.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.FromMinutes(jwtOption.ExpirationInMinutes),
                ValidIssuer = jwtOption.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtOption.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.Secret))
            };

        }); ;
    }
}
