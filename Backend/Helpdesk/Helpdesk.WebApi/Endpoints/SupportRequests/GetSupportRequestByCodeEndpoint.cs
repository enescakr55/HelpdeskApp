using FastEndpoints;
using Helpdesk.Services.ResponseModels;
using Helpdesk.Services.ResponseModels.Abstract;
using Helpdesk.Services.SupportRequests;

namespace Helpdesk.WebApi.Endpoints.SupportRequests;

public class GetSupportRequestByCodeEndpoint : EndpointWithoutRequest<IDataResult<SupportRequestTrackResponseModel>>
{
  private readonly ISupportRequestService _supportRequestService;

  public GetSupportRequestByCodeEndpoint(ISupportRequestService supportRequestService)
  {
    _supportRequestService = supportRequestService;
  }

  public override void Configure()
  {
    Get("api/support-requests/code/{requestCode}");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    var requestCode = Route<string>("requestCode");
    var result = await _supportRequestService.GetByRequestCodeAsync(requestCode, ct);
    await Send.ResponseAsync(result, result.Success ? 200 : 400, ct);
  }
}
