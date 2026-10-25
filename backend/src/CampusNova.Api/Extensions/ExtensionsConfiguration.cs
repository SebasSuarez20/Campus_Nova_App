using CampusNova.Application.Common;
using System.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using CampusNova.Realtime.Providers;
using CampusNova.Domain.Common.Interfaces;
using CampusNova.Realtime.Services;

namespace CampusNova.Api.Extensions
{
    public static class ExtensionsConfiguration
    {

        public static void AddConfigurationInvalidatorValidation(this IServiceCollection service)
        {

            service.Configure<ApiBehaviorOptions>(options =>
             {
                 options.SuppressModelStateInvalidFilter = true;
             });

        }

        public static void AddRealTime(this IServiceCollection service) {

            service.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
            service.AddScoped<INotificationService, NotificationService>();
        }
        public static async Task WarmupAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var conn = scope.ServiceProvider.GetRequiredService<IDbConnection>();
            await Task.Run(() => { if (conn.State != System.Data.ConnectionState.Open) conn.Open(); });
        }


        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .SetPreflightMaxAge(TimeSpan.FromHours(1));
                });
            });

            return services;

        }

        public static IServiceCollection AddJwtConfiguration(this IServiceCollection services,IConfiguration config)
        {

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config["Jwt:Issuer"],
                        ValidAudience = config["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(config["Jwt:Key"]!)),
                        // Sin esto, ASP.NET renombra claims custom ("institutionCode", "authId")
                        // a nombres internos de Microsoft, y FindFirst() falla.
                        NameClaimType = "authId",
                        RoleClaimType = ClaimTypes.Role
                    };
                    options.MapInboundClaims = false;


                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["access_token"];
                            return Task.CompletedTask;
                        }
                    };

                });

            return services;
        }

    }
}
