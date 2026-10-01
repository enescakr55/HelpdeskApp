using Helpdesk.DataAccess;
using Helpdesk.Entities;
using Helpdesk.Services.Departments;
using Helpdesk.Services.RequestModels;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.Tests;

public class DepartmentServiceTests
{
  [Fact]
  public async Task CreateAsync_WhenValidDepartmentName_ShouldCreateDepartment()
  {
    var options = new DbContextOptionsBuilder<PostgresDbContext>()
      .UseInMemoryDatabase(Guid.NewGuid().ToString())
      .Options;

    await using var dbContext = new PostgresDbContext(options);
    var service = new DepartmentService(dbContext);

    var result = await service.CreateAsync(new CreateDepartmentModel
    {
      DepartmentName = "IT"
    });

    Assert.True(result.Success);
    Assert.NotNull(result.Data);
    Assert.Equal("IT", result.Data.DepartmentName);
  }
}
