using Microsoft.AspNetCore.Authorization;

namespace SecureHubApi.Controllers
{
    public static class AuthorizationDemoEndpoints
    {
        public static IEndpointRouteBuilder MapAuthorizationDemoEndpoints (this IEndpointRouteBuilder app)
        {
            app.MapGet("/AdminOnly", AdminOnly);

            app.MapGet("/AdminOrTeacher", [Authorize(Roles = "Admin, Teacher")] () =>
            {
                return "AdminOrTeacher";
            });

            app.MapGet("/LibraryMemberOnly", [Authorize(Policy = "HasLibraryId")] () =>
            {
                return "Library Member";
            });

            app.MapGet("/ApplyForMaternityLeave", [Authorize(Roles = "Teacher", Policy = "FemalesOnly")] () =>
            {
                return "Applied for maternity leave";
            });

            app.MapGet("/Under10sAndFemale",
                [Authorize(Policy = "Under10")]
            [Authorize(Policy = "FemalesOnly")] () =>
                {
                    return "Under 10 and female";
                });

            return app;
        }

        [Authorize(Roles = "Admin")]
        private static string AdminOnly()
        {
            return "Admin Only";
        }
    }
}
