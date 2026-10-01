using FastEndpoints;
using Helpdesk.Services.ResponseModels;
using Helpdesk.Services.ResponseModels.Abstract;
using Helpdesk.Services.SupportRequests;

namespace Helpdesk.WebApi.Endpoints.SupportRequests;

public class AssignSupportRequestDepartmentEndpoint : EndpointWithoutRequest<IDataResult<SupportRequestResponseModel>>
{
  private readonly ISupportRequestService _supportRequestService;

  public AssignSupportRequestDepartmentEndpoint(ISupportRequestService supportRequestService)
  {
    _supportRequestService = supportRequestService;
  }

  public override void Configure()
  {
    Put("api/support-requests/{id}/assign-department/{departmentId}");
    Roles("Admin");
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    var id = Route<string>("id");
    var departmentId = Route<string>("departmentId");
    var result = await _supportRequestService.AssignDepartmentAsync(id, departmentId, ct);
    await Send.ResponseAsync(result, 200, ct);
  }
}
