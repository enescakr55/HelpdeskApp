using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Helpdesk.DataAccess;
using Helpdesk.Entities;
using Helpdesk.Services.Departments;
using Helpdesk.Services.RequestModels;
using Helpdesk.Services.ResponseModels;
using Helpdesk.Services.ResponseModels.Abstract;
using Helpdesk.Services.ResponseModels.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Helpdesk.Services.Authentication;

public class AuthService : IAuthService
{
  private readonly PostgresDbContext dbContext;
  private readonly IPasswordHasher<User> passwordHasher;
  private readonly IConfiguration configuration;
  private readonly IDepartmentService _departmentService;

  public AuthService(
    PostgresDbContext dbContext,
    IPasswordHasher<User> passwordHasher,
    IConfiguration configuration,
    IDepartmentService departmentService)
  {
    this.dbContext = dbContext;
    this.passwordHasher = passwordHasher;
    this.configuration = configuration;
    _departmentService = departmentService;
  }

  public async Task<IDataResult<AuthResponseModel>> RegisterAsync(RegisterModel model, CancellationToken cancellationToken = default)
  {
    if (model is null)
    {
      return new ErrorDataResult<AuthResponseModel>("Kayıt bilgisi boş olamaz.");
    }

    if (string.IsNullOrWhiteSpace(model.Firstname)
      || string.IsNullOrWhiteSpace(model.Lastname)
      || string.IsNullOrWhiteSpace(model.Email)
      || string.IsNullOrWhiteSpace(model.Password)
      || string.IsNullOrWhiteSpace(model.DepartmentId))
    {
      return new ErrorDataResult<AuthResponseModel>("Ad, soyad, e-posta, departman ve parola zorunludur.");
    }

    var email = model.Email.Trim();
    if (!new EmailAddressAttribute().IsValid(email))
    {
      return new ErrorDataResult<AuthResponseModel>("Geçerli bir e-posta adresi giriniz.");
    }

    if (model.Password.Length < 8)
    {
      return new ErrorDataResult<AuthResponseModel>("Parola en az 8 karakter olmalıdır.");
    }

    if (await dbContext.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower(), cancellationToken))
    {
      return new ErrorDataResult<AuthResponseModel>("Bu e-posta adresi zaten kullanılıyor.");
    }

    var department = await dbContext.Departments
      .SingleOrDefaultAsync(x => x.Id == model.DepartmentId, cancellationToken);
    if (department is null)
    {
      return new ErrorDataResult<AuthResponseModel>("Belirtilen departman bulunamadı.");
    }

    var user = new User
    {
      Id = Guid.NewGuid().ToString(),
      Firstname = model.Firstname.Trim(),
      Lastname = model.Lastname.Trim(),
      Email = email.ToLowerInvariant(),
      DepartmentId = department.Id,
      Department = department,
      IsAdmin = false
    };

    var totalUserCount = dbContext.Users.Count();
    if (totalUserCount == 0)
    {
      var departments = _departmentService.ListAsync().GetAwaiter().GetResult();
      if (departments.Data.Count() == 0)
      {
        var createdDepartment =_departmentService.CreateAsync(new CreateDepartmentModel { DepartmentName = "Genel" }).GetAwaiter().GetResult();
        departments.Data.Add(createdDepartment.Data);
      }
      user.DepartmentId = departments.Data.First().Id;
      user.IsActive = true;
      user.IsAdmin = true;
    }

    user.Password = passwordHasher.HashPassword(user, model.Password);

    dbContext.Users.Add(user);
    await dbContext.SaveChangesAsync(cancellationToken);

    var response = CreateAuthResponse(user);
    return new SuccessDataResult<AuthResponseModel>("Kayıt başarılı.", response);
  }

  public async Task<IDataResult<AuthResponseModel>> LoginAsync(LoginModel model, CancellationToken cancellationToken = default)
  {
    if (model is null)
    {
      return new ErrorDataResult<AuthResponseModel>("Giriş bilgisi boş olamaz.");
    }

    if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
    {
      return new ErrorDataResult<AuthResponseModel>("E-posta ve parola zorunludur.");
    }

    var email = model.Email.Trim();
    var user = await dbContext.Users
      .SingleOrDefaultAsync(x => x.Email.ToLower() == email.ToLower(), cancellationToken);

    if (user is null)
    {
      return new ErrorDataResult<AuthResponseModel>("E-posta veya parola hatalı.");
    }

    var verify = passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);
    if (verify == PasswordVerificationResult.Failed)
    {
      return new ErrorDataResult<AuthResponseModel>("E-posta veya parola hatalı.");
    }

    var response = CreateAuthResponse(user);
    return new SuccessDataResult<AuthResponseModel>("Giriş başarılı.", response);
  }

  private AuthResponseModel CreateAuthResponse(User user)
  {
    var token = GenerateJwtToken(user);
    return new AuthResponseModel
    {
      Token = token,
      UserId = user.Id,
      Email = user.Email,
      Firstname = user.Firstname,
      Lastname = user.Lastname,
      IsAdmin = user.IsAdmin
    };
  }

  private string GenerateJwtToken(User user)
  {
    var secret = configuration["Secret"]
      ?? throw new InvalidOperationException("Secret key is missing.");

    var key = Encoding.ASCII.GetBytes(secret);
    var claims = new List<Claim>
    {
      new Claim(JwtRegisteredClaimNames.Sub, user.Id),
      new Claim(JwtRegisteredClaimNames.Email, user.Email),
      new Claim(ClaimTypes.NameIdentifier, user.Id),
      new Claim(ClaimTypes.Email, user.Email),
      new Claim(ClaimTypes.Name, $"{user.Firstname} {user.Lastname}"),
      new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
    };

    var credentials = new SigningCredentials(
      new SymmetricSecurityKey(key),
      SecurityAlgorithms.HmacSha256Signature);

    var token = new JwtSecurityToken(
      claims: claims,
      expires: DateTime.UtcNow.AddHours(8),
      signingCredentials: credentials);

    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}
