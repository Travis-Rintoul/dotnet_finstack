
using System.Text;
using FinStack.Application.Commands;
using FinStack.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using FinStack.Infrastructure.Data;
using MediatR;
using FinStack.Domain.Repositories;
using FinStack.Infrastructure.Repositories;
using FinStack.Application.Queries;
using FinStack.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using FinStack.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("default")));

builder.Services.AddIdentity<AuthUser, AuthRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEngineService, RustFinancialEngine>();

builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }
    )
    .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = builder.Configuration["JWT:ValidAudience"],
                ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!))
            };
        }
    );

builder.Services.AddControllers(options =>
{
    options.Filters.Add<BadRequestMappingFilter>();
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(ms => ms.Value.Errors.Count > 0)
            .Select(ms => new Error(ms.Key, ms.Value.Errors.First().ErrorMessage))
            .ToList();

        var response = new ResponseMeta
        {
            Code = StatusCodes.Status400BadRequest,
            Message = "ERROR",
            Errors = errors
        };

        return new BadRequestObjectResult(response);
    };
});


builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddMediatR(typeof(GetUserByIdQuery).Assembly);
builder.Services.AddMediatR(typeof(GetAuthUserByIdQuery).Assembly);
builder.Services.AddMediatR(typeof(CreateUserCommand).Assembly);
builder.Services.AddMediatR(typeof(CreateAuthUserCommand).Assembly);
builder.Services.AddMediatR(typeof(CreateUpdateUserPreferenceCommand).Assembly);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserPreferenceRepository, UserPreferenceRepository>();
builder.Services.AddScoped<IAuthUserRepository, AuthUserRepository>();


var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
