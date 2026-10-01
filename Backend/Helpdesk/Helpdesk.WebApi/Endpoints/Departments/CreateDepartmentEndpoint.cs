using FastEndpoints;
using Helpdesk.Services.Departments;
using Helpdesk.Services.RequestModels;
using Helpdesk.Services.ResponseModels;
using Helpdesk.Services.ResponseModels.Abstract;

namespace Helpdesk.WebApi.Endpoints.Departments;

public class CreateDepartmentEndpoint : Endpoint<CreateDepartmentModel, IDataResult<DepartmentResponseModel>>
{
  private readonly IDepartmentService _departmentService;

  public CreateDepartmentEndpoint(IDepartmentService departmentService)
  {
    _departmentService = departmentService;
  }

  public override void Configure()
  {
    Post("api/departments");
    Roles("Admin");
  }

  public override async Task HandleAsync(CreateDepartmentModel req, CancellationToken ct)
  {
    var result = await _departmentService.CreateAsync(req, ct);
    await Send.ResponseAsync(result, result.Success ? 200 : 400, ct);
  }
}
