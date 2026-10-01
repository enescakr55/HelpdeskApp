
using FastEndpoints;
using Helpdesk.DataAccess;
using Helpdesk.Entities;
using Helpdesk.Services.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Helpdesk.Services.Departments;
using Helpdesk.Services.SupportRequests;

namespace Helpdesk.WebApi
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);
      builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
        .AddEnvironmentVariables()
        .Build();
      var Configuration = builder.Configuration;
      // Add services to the container.
      var secret = Configuration.GetRequiredSection("Secret");
      if(secret == null){
        throw new Exception("Secret key cannot be null");
      }
      var key = Encoding.ASCII.GetBytes(secret.Value!.ToString());
      builder.Services.AddAuthentication(x =>
      {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(x =>
      {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = false,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateIssuer = false,
          ValidateAudience = false
        };

      });
      builder.Services.AddAuthorization();
      builder.Services.AddFastEndpoints();
      
      builder.Services.AddScoped<IUserService, UserService>();
      builder.Services.AddScoped<IDepartmentService, DepartmentService>();
      builder.Services.AddScoped<ISupportRequestService, SupportRequestService>();
      builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
      var connectionString = Configuration.GetConnectionString("Default")
        ?? throw new InvalidOperationException("Connection string 'Default' was not found.");
      builder.Services.AddDbContext<PostgresDbContext>(options =>
        options.UseNpgsql(connectionString));
      
      // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen();

      builder.Services.AddSingleton<IConfiguration>(Configuration);
      var app = builder.Build();

      // Configure the HTTP request pipeline.
      if (app.Environment.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }

      app.UseHttpsRedirection();
      app.UseAuthentication();
      app.UseAuthorization();
      app.UseFastEndpoints();
      var summaries = new[]
      {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

      app.MapGet("/weatherforecast", (HttpContext httpContext) =>
      {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                  new WeatherForecast
                  {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = summaries[Random.Shared.Next(summaries.Length)]
                  })
                  .ToArray();
        return forecast;
      })
      .WithName("GetWeatherForecast")
      .WithOpenApi();

      app.Run();
    }
  }
}
