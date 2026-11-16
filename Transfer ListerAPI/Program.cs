using Microsoft.EntityFrameworkCore;
using Transfer_ListerAPI;
using Transfer_ListerAPI.Data;
using Transfer_ListerAPI.Repository;
using Transfer_ListerAPI.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("SqlServerConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'SqlServerConnection' is not configured.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddAutoMapper(_ => { }, typeof(AutoMapperConfiguration).Assembly);
builder.Services.AddControllers();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IPositionRepository, PositionRepository>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
