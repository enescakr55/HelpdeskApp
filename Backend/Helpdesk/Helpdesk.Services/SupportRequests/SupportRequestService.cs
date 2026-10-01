using System.ComponentModel.DataAnnotations;
using Helpdesk.DataAccess;
using Helpdesk.Entities;
using Helpdesk.Entities.Flags;
using Helpdesk.Services.RequestModels;
using Helpdesk.Services.ResponseModels;
using Helpdesk.Services.ResponseModels.Abstract;
using Helpdesk.Services.ResponseModels.Concrete;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.Services.SupportRequests;

public class SupportRequestService : ISupportRequestService
{
  private readonly PostgresDbContext dbContext;

  public SupportRequestService(PostgresDbContext dbContext)
  {
    this.dbContext = dbContext;
  }

  public async Task<IDataResult<SupportRequestResponseModel>> CreateAsync(CreateSupportRequestModel model, CancellationToken cancellationToken = default)
  {
    if (model is null)
    {
      return new ErrorDataResult<SupportRequestResponseModel>("Talep bilgisi boş olamaz.");
    }

    var fullname = model.Fullname?.Trim();
    var email = model.Email?.Trim();
    var title = model.Title?.Trim();
    var description = model.Description?.Trim();

    if (string.IsNullOrWhiteSpace(fullname))
    {
      return new ErrorDataResult<SupportRequestResponseModel>("Ad soyad zorunludur.");
    }

    if (string.IsNullOrWhiteSpace(email) || !new EmailAddressAttribute().IsValid(email))
    {
      return new ErrorDataResult<SupportRequestResponseModel>("Geçerli bir e-posta adresi giriniz.");
    }

    if (string.IsNullOrWhiteSpace(title))
    {
      return new ErrorDataResult<SupportRequestResponseModel>("Konu başlığı zorunludur.");
    }

    if (string.IsNullOrWhiteSpace(description))
    {
      return new ErrorDataResult<SupportRequestResponseModel>("Talep açıklaması zorunludur.");
    }

    var requestCode = await GenerateUniqueRequestCodeAsync(cancellationToken);

    var request = new SupportRequest
    {
      Id = Guid.NewGuid().ToString(),
      RequestCode = requestCode,
      Fullname = fullname,
      Email = email.ToLowerInvariant(),
      Subject = model.Subject,
      Priority = model.Priority,
      Title = title,
      Description = description,
      Status = SupportTypeStatusEnum.Open
    };

    dbContext.SupportRequests.Add(request);
    await dbContext.SaveChangesAsync(cancellationToken);

    return new SuccessDataResult<SupportRequestResponseModel>("Destek talebi oluşturuldu.", MapToResponse(request));
  }

  public async Task<IDataResult<SupportRequestTrackResponseModel>> GetByRequestCodeAsync(string requestCode, CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(requestCode))
    {
      return new ErrorDataResult<SupportRequestTrackResponseModel>("Talep kodu zorunludur.");
    }

    var request = await dbContext.SupportRequests
      .AsNoTracking()
      .SingleOrDefaultAsync(x => x.RequestCode == requestCode.Trim(), cancellationToken);

    if (request is null)
    {
      return new ErrorDataResult<SupportRequestTrackResponseModel>("Bu talep koduna ait kayıt bulunamadı.");
    }

    var response = new SupportRequestTrackResponseModel
    {
      RequestCode = request.RequestCode,
      Subject = request.Subject,
      Priority = request.Priority,
      Title = request.Title,
      Description = request.Description,
      Status = request.Status,
      UserMessage = request.UserMessage
    };

    return new SuccessDataResult<SupportRequestTrackResponseModel>("Talep bulundu.", response);
  }

  public async Task<IDataResult<List<SupportRequestResponseModel>>> GetByUserAsync(string email, CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(email))
    {
      return new ErrorDataResult<List<SupportRequestResponseModel>>("E-posta adresi zorunludur.");
    }

    var requests = await dbContext.SupportRequests
      .AsNoTracking()
      .Include(x => x.AssignedDepartment)
      .Where(x => x.Email.ToLower() == email.Trim().ToLower())
      .OrderByDescending(x => x.Id)
      .Select(x => new SupportRequestResponseModel
      {
        Id = x.Id,
        RequestCode = x.RequestCode,
        Fullname = x.Fullname,
        Email = x.Email,
        Subject = x.Subject,
        Priority = x.Priority,
        Title = x.Title,
        Description = x.Description,
        Status = x.Status,
        AssignedDepartmentId = x.AssignedDepartmentId,
        AssignMessage = x.AssignMessage,
        UserMessage = x.UserMessage,
        AssignedDepartmentName = x.AssignedDepartment != null ? x.AssignedDepartment.DepartmentName : string.Empty
      })
      .ToListAsync(cancellationToken);

    return new SuccessDataResult<List<SupportRequestResponseModel>>("Talep listesi getirildi.", requests);
  }

  public async Task<IDataResult<List<SupportRequestResponseModel>>> GetAllForAdminAsync(CancellationToken cancellationToken = default)
  {
    var requests = await dbContext.SupportRequests
      .AsNoTracking()
      .Include(x => x.AssignedDepartment)
      .OrderByDescending(x => x.Id)
      .Select(x => new SupportRequestResponseModel
      {
        Id = x.Id,
        RequestCode = x.RequestCode,
        Fullname = x.Fullname,
        Email = x.Email,
        Subject = x.Subject,
        Priority = x.Priority,
        Title = x.Title,
        Description = x.Description,
        Status = x.Status,
        AssignedDepartmentId = x.AssignedDepartmentId,
        AssignMessage = x.AssignMessage,
        UserMessage = x.UserMessage,
        AssignedDepartmentName = x.AssignedDepartment != null ? x.AssignedDepartment.DepartmentName : string.Empty
      })
      .ToListAsync(cancellationToken);

    return new SuccessDataResult<List<SupportRequestResponseModel>>("Tüm destek talepleri listelendi.", requests);
  }

  public async Task<IDataResult<SupportRequestResponseModel>> UpdateStatusAsync(string id, UpdateSupportRequestStatusModel model, CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(id))
    {
      return new ErrorDataResult<SupportRequestResponseModel>("Talep Id zorunludur.");
    }

    if (model is null)
    {
      return new ErrorDataResult<SupportRequestResponseModel>("Durum bilgisi boş olamaz.");
    }

    var request = await dbContext.SupportRequests
      .Include(x => x.AssignedDepartment)
      .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    if (request is null)
    {
      return new ErrorDataResult<SupportRequestResponseModel>("Talep bulunamadı.");
    }

    request.Status = model.Status;
    await dbContext.SaveChangesAsync(cancellationToken);

    return new SuccessDataResult<SupportRequestResponseModel>("Talep durumu güncellendi.", MapToResponse(request));
  }

  public async Task<IDataResult<SupportRequestResponseModel>> AddMessageAsync(string id, AddSupportRequestMessageModel model, CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(id))
    {
      return new ErrorDataResult<SupportRequestResponseModel>("Talep Id zorunludur.");
    }

    if (model is null)
    {
      return new ErrorDataResult<SupportRequestResponseModel>("Mesaj bilgisi boş olamaz.");
    }

    var message = model.Message?.Trim();
    if (string.IsNullOrWhiteSpace(message))
    {
      return new ErrorDataResult<SupportRequestResponseModel>("Mesaj boş olamaz.");
    }

    var request = await dbContext.SupportRequests
      .Include(x => x.AssignedDepartment)
      .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    if (request is null)
    {
      return new ErrorDataResult<SupportRequestResponseModel>("Talep bulunamadı.");
    }

    if (model.IsUserMessage)
    {
      request.UserMessage = message;
      request.Status = SupportTypeStatusEnum.WaitingForUser;
    }
    else
    {
      request.AssignMessage = message;
      request.Status = SupportTypeStatusEnum.InProgress;
    }

    await dbContext.SaveChangesAsync(cancellationToken);

    return new SuccessDataResult<SupportRequestResponseModel>("Mesaj eklendi.", MapToResponse(request));
  }

  public async Task<IDataResult<SupportRequestResponseModel>> AssignDepartmentAsync(string id, string departmentId, CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(id))
    {
      return new ErrorDataResult<SupportRequestResponseModel>("Talep Id zorunludur.");
    }

    if (string.IsNullOrWhiteSpace(departmentId))
    {
      return new ErrorDataResult<SupportRequestResponseModel>("Departman Id zorunludur.");
    }

    var request = await dbContext.SupportRequests
      .Include(x => x.AssignedDepartment)
      .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    if (request is null)
    {
      return new ErrorDataResult<SupportRequestResponseModel>("Talep bulunamadı.");
    }

    var department = await dbContext.Departments
      .SingleOrDefaultAsync(x => x.Id == departmentId, cancellationToken);

    if (department is null)
    {
      return new ErrorDataResult<SupportRequestResponseModel>("Belirtilen departman bulunamadı.");
    }

    request.AssignedDepartmentId = department.Id;
    request.AssignedDepartment = department;
    request.Status = SupportTypeStatusEnum.InProgress;

    await dbContext.SaveChangesAsync(cancellationToken);

    return new SuccessDataResult<SupportRequestResponseModel>("Talep departmana atandı.", MapToResponse(request));
  }

  private async Task<string> GenerateUniqueRequestCodeAsync(CancellationToken cancellationToken)
  {
    var code = string.Empty;

    while (true)
    {
      code = $"SR-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N")[..16].ToUpper()}";

      var exists = await dbContext.SupportRequests.AnyAsync(x => x.RequestCode == code, cancellationToken);
      if (!exists)
      {
        return code;
      }
    }
  }

  private static SupportRequestResponseModel MapToResponse(SupportRequest request)
  {
    return new SupportRequestResponseModel
    {
      Id = request.Id,
      RequestCode = request.RequestCode,
      Fullname = request.Fullname,
      Email = request.Email,
      Subject = request.Subject,
      Priority = request.Priority,
      Title = request.Title,
      Description = request.Description,
      Status = request.Status,
      AssignedDepartmentId = request.AssignedDepartmentId,
      AssignMessage = request.AssignMessage,
      UserMessage = request.UserMessage,
      AssignedDepartmentName = request.AssignedDepartment != null ? request.AssignedDepartment.DepartmentName : string.Empty
    };
  }
}
