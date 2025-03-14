using Microsoft.EntityFrameworkCore;
using SecureHubApi.Models;

namespace SecureHubApi.Extensions
{
    public static class EFCoreExtensions
    {
        public static IServiceCollection InjectDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                   options.UseSqlServer(configuration.GetConnectionString("DevDb")));

            return services;
        }
    }
}
