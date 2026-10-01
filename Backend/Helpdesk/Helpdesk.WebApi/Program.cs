
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
using Helpdesk.Services.Authentication;
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
      builder.Services.AddCors(options =>
      {
        options.AddPolicy(name: "AllowedCorsOrigins",
            builder =>
            {
              builder
                            .SetIsOriginAllowed(origin => true)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
            });
      });
      builder.Services.AddAuthorization();
      builder.Services.AddFastEndpoints();
      
      builder.Services.AddScoped<IUserService, UserService>();
      builder.Services.AddScoped<IAuthService, AuthService>();
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

      using (var scope = app.Services.CreateScope())
      {
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var dbContext = scope.ServiceProvider.GetRequiredService<PostgresDbContext>();
        dbContext.Database.Migrate();

        var departmentService = scope.ServiceProvider.GetRequiredService<IDepartmentService>();
        var departments = departmentService.ListAsync().GetAwaiter().GetResult();
        if(departments.Success && departments.Data.Count() == 0){
          var initialName = configuration.GetRequiredSection("General:InitialDepartmentName").Value!.ToString();
          departmentService.CreateAsync(new Services.RequestModels.CreateDepartmentModel
          {
            DepartmentName = initialName
          }).GetAwaiter().GetResult();
        }
      }

      // Configure the HTTP request pipeline.
      if (app.Environment.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }
      app.UseCors("AllowedCorsOrigins");
      app.UseHttpsRedirection();
      app.UseAuthentication();
      app.UseAuthorization();
      app.UseFastEndpoints();

      app.Run();
    }
  }
}
