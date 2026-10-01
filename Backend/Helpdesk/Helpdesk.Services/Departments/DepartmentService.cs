using Helpdesk.DataAccess;
using Helpdesk.Entities;
using Helpdesk.Services.RequestModels;
using Helpdesk.Services.ResponseModels;
using Helpdesk.Services.ResponseModels.Abstract;
using Helpdesk.Services.ResponseModels.Concrete;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.Services.Departments;

public class DepartmentService : IDepartmentService
{
  private readonly PostgresDbContext dbContext;

  public DepartmentService(PostgresDbContext dbContext)
  {
    this.dbContext = dbContext;
  }

  public async Task<IDataResult<List<DepartmentResponseModel>>> ListAsync(CancellationToken cancellationToken = default)
  {
    var departments = await dbContext.Departments
      .AsNoTracking()
      .OrderBy(x => x.DepartmentName)
      .Select(x => new DepartmentResponseModel
      {
        Id = x.Id,
        DepartmentName = x.DepartmentName
      })
      .ToListAsync(cancellationToken);

    return new SuccessDataResult<List<DepartmentResponseModel>>("Departmanlar listelendi.", departments);
  }

  public async Task<IDataResult<DepartmentResponseModel>> CreateAsync(CreateDepartmentModel model, CancellationToken cancellationToken = default)
  {
    if (model is null)
    {
      return new ErrorDataResult<DepartmentResponseModel>("Departman bilgisi boş olamaz.");
    }

    var departmentName = model.DepartmentName?.Trim();
    if (string.IsNullOrWhiteSpace(departmentName))
    {
      return new ErrorDataResult<DepartmentResponseModel>("Departman adı zorunludur.");
    }

    if (departmentName.Length < 2)
    {
      return new ErrorDataResult<DepartmentResponseModel>("Departman adı en az 2 karakter olmalıdır.");
    }

    var exists = await dbContext.Departments.AnyAsync(x => x.DepartmentName.ToLower() == departmentName.ToLower(), cancellationToken);
    if (exists)
    {
      return new ErrorDataResult<DepartmentResponseModel>("Bu departman adı zaten mevcut.");
    }

    var department = new Department
    {
      Id = Guid.NewGuid().ToString(),
      DepartmentName = departmentName
    };

    dbContext.Departments.Add(department);
    await dbContext.SaveChangesAsync(cancellationToken);

    return new SuccessDataResult<DepartmentResponseModel>("Departman oluşturuldu.", MapToResponse(department));
  }

  public async Task<IDataResult<DepartmentResponseModel>> UpdateAsync(string id, UpdateDepartmentModel model, CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(id))
    {
      return new ErrorDataResult<DepartmentResponseModel>("Departman Id zorunludur.");
    }

    if (model is null)
    {
      return new ErrorDataResult<DepartmentResponseModel>("Departman bilgisi boş olamaz.");
    }

    var departmentName = model.DepartmentName?.Trim();
    if (string.IsNullOrWhiteSpace(departmentName))
    {
      return new ErrorDataResult<DepartmentResponseModel>("Departman adı zorunludur.");
    }

    var department = await dbContext.Departments
      .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    if (department is null)
    {
      return new ErrorDataResult<DepartmentResponseModel>("Departman bulunamadı.");
    }

    var exists = await dbContext.Departments.AnyAsync(
      x => x.Id != id && x.DepartmentName.ToLower() == departmentName.ToLower(),
      cancellationToken);

    if (exists)
    {
      return new ErrorDataResult<DepartmentResponseModel>("Bu departman adı zaten kullanılmaktadır.");
    }

    department.DepartmentName = departmentName;
    await dbContext.SaveChangesAsync(cancellationToken);

    return new SuccessDataResult<DepartmentResponseModel>("Departman güncellendi.", MapToResponse(department));
  }

  public async Task<IDataResult<bool>> DeleteAsync(string id, CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(id))
    {
      return new ErrorDataResult<bool>("Departman Id zorunludur.");
    }

    var department = await dbContext.Departments
      .Include(x => x.Users)
      .Include(x => x.SupportRequests)
      .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    if (department is null)
    {
      return new ErrorDataResult<bool>("Departman bulunamadı.");
    }

    if (department.Users.Any() || department.SupportRequests.Any())
    {
      return new ErrorDataResult<bool>("Bu departman üzerinde kullanıcı veya talep bulunduğu için silinemez.");
    }

    dbContext.Departments.Remove(department);
    await dbContext.SaveChangesAsync(cancellationToken);

    return new SuccessDataResult<bool>("Departman silindi.", true);
  }

  private static DepartmentResponseModel MapToResponse(Department department)
  {
    return new DepartmentResponseModel
    {
      Id = department.Id,
      DepartmentName = department.DepartmentName
    };
  }
}
