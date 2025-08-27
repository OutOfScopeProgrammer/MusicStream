using System.Reflection;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Music.API.Authorizarion;
using Music.API.Authorization;
using Music.API.Helper;
using Music.API.Interfaces;
using MusicStream.Infrastructure.Auth;

namespace Music.API.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApiLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers().ConfigureApiBehaviorOptions(option =>
        {
            option.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState.Where(x => x.Value?.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToList());
                var response = ApiResponse<object>.BadRequest(errors);
                return new BadRequestObjectResult(response);

            };
        });
        services.AddAuth(configuration);
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
        .AddFluentValidationAutoValidation();
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

            var jwtOption = configuration.GetSection(nameof(JwtOption)).Get<JwtOption>()
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
            services.AddAuthorization(p =>
            {
                p.AddPolicy(AuthPolicy.Admin.ToString(), p => p.RequireRole(ApplicationRoles.ADMIN.ToString()));
                p.AddPolicy(AuthPolicy.User.ToString(), p =>
                p.RequireRole(ApplicationRoles.USER.ToString(), ApplicationRoles.ADMIN.ToString()));
            });

        }); ;
    }
}
