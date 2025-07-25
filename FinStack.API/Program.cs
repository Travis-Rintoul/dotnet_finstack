
using Microsoft.EntityFrameworkCore;
using FinStack.Infrastructure.Data;
using MediatR;
using FinStack.Domain.Repositories;
using FinStack.Infrastructure.Repositories;
using FinStack.Application.Queries;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddMediatR(typeof(GetUsersQueryHandler).Assembly);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql("Host=localhost;Port=5432;Database=finstack_db;Username=postgres");
});

var app = builder.Build();

app.MapControllers();

app.Run();
