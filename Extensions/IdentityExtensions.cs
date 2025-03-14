using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SecureHubApi.Models;
using System.Text;

namespace SecureHubApi.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddEntityHandlersAndStores(this IServiceCollection services)
        {
            services.AddIdentityApiEndpoints<User>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>();

            return services;    
        }

        public static IServiceCollection ConfigureIdentityOptions(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddIdentityAuth(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(y =>
            {
                y.SaveToken = false;
                y.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        config["AppSettings:JwtSecret"]!)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();

                options.AddPolicy("HasLibraryId", policy => policy.RequireClaim("LibraryId"));  
                options.AddPolicy("FemalesOnly", policy => policy.RequireClaim("Gender", "Female"));
                options.AddPolicy("Under10", policy => policy.RequireAssertion(
                    context => Int32.Parse(context.User.Claims.First(x => x.Type == "Age").Value)<10)
                );

            });
            return services;
        }

        public static WebApplication AddIdentityAuthMiddlewares (this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }
    }
}
