using FastEndpoints;
using Helpdesk.Services.ResponseModels;
using Helpdesk.Services.ResponseModels.Abstract;
using Helpdesk.Services.SupportRequests;

namespace Helpdesk.WebApi.Endpoints.SupportRequests;

public class GetSupportRequestByCodeEndpoint : EndpointWithoutRequest<IDataResult<SupportRequestResponseModel>>
{
  private readonly ISupportRequestService _supportRequestService;

  public GetSupportRequestByCodeEndpoint(ISupportRequestService supportRequestService)
  {
    _supportRequestService = supportRequestService;
  }

  public override void Configure()
  {
    Get("api/support-requests/code/{requestCode}");
    Roles("User", "Admin");
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    var requestCode = Route<string>("requestCode");
    var result = await _supportRequestService.GetByRequestCodeAsync(requestCode, ct);
    await Send.ResponseAsync(result, 200, ct);
  }
}
