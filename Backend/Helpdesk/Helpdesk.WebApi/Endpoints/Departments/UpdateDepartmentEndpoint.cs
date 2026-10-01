using FastEndpoints;
using Helpdesk.Services.Departments;
using Helpdesk.Services.RequestModels;
using Helpdesk.Services.ResponseModels;
using Helpdesk.Services.ResponseModels.Abstract;

namespace Helpdesk.WebApi.Endpoints.Departments;

public class UpdateDepartmentEndpoint : Endpoint<UpdateDepartmentModel, IDataResult<DepartmentResponseModel>>
{
  private readonly IDepartmentService _departmentService;

  public UpdateDepartmentEndpoint(IDepartmentService departmentService)
  {
    _departmentService = departmentService;
  }

  public override void Configure()
  {
    Post("api/departments/update/{id}");
    Roles("Admin");
  }

  public override async Task HandleAsync(UpdateDepartmentModel req, CancellationToken ct)
  {
    var id = Route<string>("id");
    var result = await _departmentService.UpdateAsync(id, req, ct);
    await Send.ResponseAsync(result, result.Success ? 200 : 400, ct);
  }
}
