using System.ComponentModel.DataAnnotations;
using Helpdesk.DataAccess;
using Helpdesk.Entities;
using Helpdesk.Services.RequestModels;
using Helpdesk.Services.ResponseModels;
using Helpdesk.Services.ResponseModels.Abstract;
using Helpdesk.Services.ResponseModels.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.Services.Users;

public class UserService : IUserService
{
  private readonly PostgresDbContext dbContext;
  private readonly IPasswordHasher<User> passwordHasher;
  public UserService(PostgresDbContext _dbContext, IPasswordHasher<User> _passwordHasher)
  {
    this.dbContext = _dbContext;
    this.passwordHasher = _passwordHasher;
  }
  public async Task<IDataResult<List<UserResponseModel>>> ListAsync()
  {
    var users = await dbContext.Users.Select(x => new UserResponseModel
    {
      Id = x.Id,
      DepartmentId = x.DepartmentId,
      Email = x.Email,
      Firstname = x.Firstname,
      Lastname = x.Lastname,
      IsAdmin = x.IsAdmin
    })
    .ToListAsync();
    return new SuccessDataResult<List<UserResponseModel>>("Kullanıcılar listelendi",users);

  }
  public async Task<IDataResult<UserResponseModel>> CreateAsync(
    CreateUserModel model,
    CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(model.Firstname)
      || string.IsNullOrWhiteSpace(model.Lastname)
      || string.IsNullOrWhiteSpace(model.Email)
      || string.IsNullOrWhiteSpace(model.DepartmentId)
      || model.Password.Length < 8)
    {
      return new ErrorDataResult<UserResponseModel>(
        "Ad, soyad, e-posta ve departman zorunludur; parola en az 8 karakter olmalıdır.");
    }

    var email = model.Email.Trim().ToLowerInvariant();
    if (!new EmailAddressAttribute().IsValid(email))
    {
      return new ErrorDataResult<UserResponseModel>("Geçerli bir e-posta adresi giriniz.");
    }

    if (await dbContext.Users.AnyAsync(user => user.Email == email, cancellationToken))
    {
      return new ErrorDataResult<UserResponseModel>("Bu e-posta adresi zaten kullanılıyor.");
    }

    var department = await dbContext.Departments
      .SingleOrDefaultAsync(item => item.Id == model.DepartmentId, cancellationToken);
    if (department is null)
    {
      return new ErrorDataResult<UserResponseModel>("Belirtilen departman bulunamadı.");
    }

    var user = new User
    {
      Id = Guid.NewGuid().ToString(),
      Firstname = model.Firstname.Trim(),
      Lastname = model.Lastname.Trim(),
      Email = email,
      DepartmentId = department.Id,
      Department = department,
      IsAdmin = false
    };
    user.Password = passwordHasher.HashPassword(user, model.Password);

    dbContext.Users.Add(user);
    await dbContext.SaveChangesAsync(cancellationToken);

    var response = new UserResponseModel(
      user.Id,
      user.Firstname,
      user.Lastname,
      user.Email,
      user.DepartmentId,
      user.IsAdmin);

    return new SuccessDataResult<UserResponseModel>("Kullanıcı oluşturuldu.", response);
  }
}