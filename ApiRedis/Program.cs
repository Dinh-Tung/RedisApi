using ApiRedis.ApplicationServices.implements;
using ApiRedis.ApplicationServices.Interfaces;
using ApiRedis.DatabaseContext;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
IConfiguration configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped < ICacheService, CacheService > ();
builder.Services.AddDbContext<DbContextClass>(option => option.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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