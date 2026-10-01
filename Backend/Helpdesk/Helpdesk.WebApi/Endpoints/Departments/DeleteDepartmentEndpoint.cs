using FastEndpoints;
using Helpdesk.Services.Departments;
using Helpdesk.Services.ResponseModels.Abstract;

namespace Helpdesk.WebApi.Endpoints.Departments;

public class DeleteDepartmentEndpoint : EndpointWithoutRequest<IDataResult<bool>>
{
  private readonly IDepartmentService _departmentService;

  public DeleteDepartmentEndpoint(IDepartmentService departmentService)
  {
    _departmentService = departmentService;
  }

  public override void Configure()
  {
    Delete("api/departments/{id}");
    Roles("Admin");
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    var id = Route<string>("id");
    var result = await _departmentService.DeleteAsync(id, ct);
    await Send.ResponseAsync(result, 200, ct);
  }
}
