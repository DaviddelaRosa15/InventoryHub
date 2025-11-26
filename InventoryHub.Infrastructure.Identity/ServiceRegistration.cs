using InventoryHub.Core.Application.Constants;
using InventoryHub.Core.Application.Helpers;
using InventoryHub.Core.Application.Interfaces.Services;
using InventoryHub.Core.Domain.Settings;
using InventoryHub.Infrastructure.Identity.Contexts;
using InventoryHub.Infrastructure.Identity.Entities;
using InventoryHub.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

namespace InventoryHub.Infrastructure.Identity
{
    public static class ServiceRegistration
    {
        public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Vaciar tablas
            /*var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>();
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseNpgsql(connection, m => m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName));
            var context = new IdentityContext(optionsBuilder.Options);
			context.TruncateTables();*/
            #endregion

            #region Contexts
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<IdentityContext>(options => options.UseInMemoryDatabase("BaseDb"));
            }
            else
            {
                var connection = configuration.GetConnectionString("IdentityDB");
                var parameters = configuration["IdentityDB"];
                connection = connection.Replace("%IdentityDB%", parameters);

                services.AddDbContext<IdentityContext>(options =>
                {
                    options.EnableSensitiveDataLogging();
                    options.UseNpgsql(connection,
                    m => m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName));
                });
            }
            #endregion

            #region Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/User";
                options.AccessDeniedPath = "/User/AccessDenied";
            });

            // Access to replace placeholders
            var jwtSettingsSection = configuration.GetSection("JWTSettings");
            var jwtSettings = jwtSettingsSection.Get<JWTSettings>();
            var refreshSettingsSection = configuration.GetSection("RefreshJWTSettings");
            var refreshSettings = refreshSettingsSection.Get<RefreshJWTSettings>();

            // Replace placeholders in settings
            jwtSettings.Key = configuration["JWTKEY"];
            refreshSettings.Key = configuration["REFRESHKEY"];

            // Configuring JWT settings
            services.Configure<JWTSettings>(options =>
            {
                options.Key = jwtSettings.Key;
                options.Issuer = jwtSettings.Issuer;
                options.Audience = jwtSettings.Audience;
                options.DurationInMinutes = jwtSettings.DurationInMinutes;
            });

            // Configuring RefreshJWT settings
            services.Configure<RefreshJWTSettings>(options =>
            {
                options.Key = refreshSettings.Key;
                options.Issuer = refreshSettings.Issuer;
                options.Audience = refreshSettings.Audience;
                options.DurationInMinutes = refreshSettings.DurationInMinutes;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                };
                options.Events = new JwtBearerEvents()
                {
                    OnChallenge = c =>
                    {
                        c.HandleResponse();
                        c.Response.StatusCode = 401;
                        c.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(ErrorMapperHelper.Error(ErrorMessages.Unauthorized, "Usuario no autorizado"));
                        return c.Response.WriteAsync(result);
                    },
                    OnForbidden = c =>
                    {
                        c.Response.StatusCode = 403;
                        c.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(ErrorMapperHelper.Error(ErrorMessages.Unauthorized, "Usted no está autorizado para usar este endpoint"));
                        return c.Response.WriteAsync(result);
                    }
                };

            });
            #endregion

            #region Services
            services.AddTransient<IAccountService, AccountService>();
            #endregion
        }
    }
}
