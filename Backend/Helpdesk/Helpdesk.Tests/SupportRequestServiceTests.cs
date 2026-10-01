using Helpdesk.DataAccess;
using Helpdesk.Entities;
using Helpdesk.Entities.Flags;
using Helpdesk.Services.Departments;
using Helpdesk.Services.RequestModels;
using Helpdesk.Services.SupportRequests;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.Tests;

public class SupportRequestServiceTests
{
  [Fact]
  public async Task CreateAsync_WhenValidInput_ShouldCreateRequestAndGenerateCode()
  {
    var options = new DbContextOptionsBuilder<PostgresDbContext>()
      .UseInMemoryDatabase(Guid.NewGuid().ToString())
      .Options;

    await using var dbContext = new PostgresDbContext(options);
    var service = new SupportRequestService(dbContext);

    var result = await service.CreateAsync(new CreateSupportRequestModel
    {
      Fullname = "Ali Veli",
      Email = "ali@demo.com",
      Subject = 1,
      Priority = 2,
      Title = "Test",
      Description = "Açıklama"
    });

    Assert.True(result.Success);
    Assert.NotNull(result.Data);
    Assert.False(string.IsNullOrWhiteSpace(result.Data.RequestCode));
    Assert.Equal(SupportTypeStatusEnum.Open, result.Data.Status);
  }

  [Fact]
  public async Task AssignDepartmentAsync_WhenDepartmentExists_ShouldAssignDepartment()
  {
    var options = new DbContextOptionsBuilder<PostgresDbContext>()
      .UseInMemoryDatabase(Guid.NewGuid().ToString())
      .Options;

    await using var dbContext = new PostgresDbContext(options);
    var department = new Department
    {
      Id = Guid.NewGuid().ToString(),
      DepartmentName = "IT"
    };
    dbContext.Departments.Add(department);
    await dbContext.SaveChangesAsync();

    var service = new SupportRequestService(dbContext);
    var created = await service.CreateAsync(new CreateSupportRequestModel
    {
      Fullname = "Ali Veli",
      Email = "ali@demo.com",
      Subject = 1,
      Priority = 2,
      Title = "Test",
      Description = "Açıklama"
    });

    var result = await service.AssignDepartmentAsync(created.Data.Id, department.Id);

    Assert.True(result.Success);
    Assert.Equal(department.Id, result.Data.AssignedDepartmentId);
  }
}
