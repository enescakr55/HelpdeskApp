using FastEndpoints;
using Helpdesk.Services.ResponseModels;
using Helpdesk.Services.ResponseModels.Abstract;
using Helpdesk.Services.SupportRequests;

namespace Helpdesk.WebApi.Endpoints.SupportRequests;

public class GetUserSupportRequestsEndpoint : EndpointWithoutRequest<IDataResult<List<SupportRequestResponseModel>>>
{
  private readonly ISupportRequestService _supportRequestService;

  public GetUserSupportRequestsEndpoint(ISupportRequestService supportRequestService)
  {
    _supportRequestService = supportRequestService;
  }

  public override void Configure()
  {
    Get("api/support-requests/user/{email}");
    Roles("User", "Admin");
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    var email = Route<string>("email");
    var result = await _supportRequestService.GetByUserAsync(email, ct);
    await Send.ResponseAsync(result, 200, ct);
  }
}
