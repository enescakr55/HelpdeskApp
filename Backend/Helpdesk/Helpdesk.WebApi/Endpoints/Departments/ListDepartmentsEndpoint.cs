using FastEndpoints;
using Helpdesk.Services.Departments;
using Helpdesk.Services.ResponseModels;
using Helpdesk.Services.ResponseModels.Abstract;

namespace Helpdesk.WebApi.Endpoints.Departments;

public class ListDepartmentsEndpoint : EndpointWithoutRequest<IDataResult<List<DepartmentResponseModel>>>
{
  private readonly IDepartmentService _departmentService;

  public ListDepartmentsEndpoint(IDepartmentService departmentService)
  {
    _departmentService = departmentService;
  }

  public override void Configure()
  {
    Get("api/departments");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    var result = await _departmentService.ListAsync(ct);
    await Send.ResponseAsync(result, result.Success ? 200 : 400, ct);
  }
}
