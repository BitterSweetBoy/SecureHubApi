using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SecureHubApi.Controllers;
using SecureHubApi.Extensions;
using SecureHubApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerExplorer()
                .InjectDbContext(builder.Configuration)
                .AddAppConfig(builder.Configuration)
                .AddEntityHandlersAndStores()
                .AddIdentityAuth(builder.Configuration);



var app = builder.Build();

app.ConfigureSwaggerExplore()
    .ConfigCors(builder.Configuration)
    .AddIdentityAuthMiddlewares();


app.UseHttpsRedirection();


app.MapControllers();
app.MapGroup("/api")
   .MapIdentityApi<User>();

app.MapGroup("/api")
   .MapIdentityUserEndpoints()
   .MappAccountEndpoints()
   .MapAuthorizationDemoEndpoints();


app.Run();

