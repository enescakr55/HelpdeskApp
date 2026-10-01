using FastEndpoints;
using Helpdesk.Services.ResponseModels;
using Helpdesk.Services.ResponseModels.Abstract;
using Helpdesk.Services.SupportRequests;

namespace Helpdesk.WebApi.Endpoints.SupportRequests;

public class GetAdminSupportRequestsEndpoint : EndpointWithoutRequest<IDataResult<List<SupportRequestResponseModel>>>
{
  private readonly ISupportRequestService _supportRequestService;

  public GetAdminSupportRequestsEndpoint(ISupportRequestService supportRequestService)
  {
    _supportRequestService = supportRequestService;
  }

  public override void Configure()
  {
    Get("api/support-requests");
    Roles("Admin");
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    var result = await _supportRequestService.GetAllForAdminAsync(ct);
    await Send.ResponseAsync(result, result.Success ? 200 : 400, ct);
  }
}
