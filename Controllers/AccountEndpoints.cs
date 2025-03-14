using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SecureHubApi.Models;
using System.Security.Claims;

namespace SecureHubApi.Controllers
{
    public static class AccountEndpoints
    {
        public static IEndpointRouteBuilder MappAccountEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/profile", GetUserProfile);
            return app;
        }


        [Authorize]
        private static async Task<IResult> GetUserProfile(ClaimsPrincipal user, UserManager<User> userManager)
        {
            string userId = user.Claims.First(x => x.Type == "UserId").Value;
            var userDetails = await userManager.FindByIdAsync(userId);
            return Results.Ok(
                new
                {
                    Email = userDetails?.Email,
                    FullName = userDetails?.FullName,
                }
                );
        }
    }
}
