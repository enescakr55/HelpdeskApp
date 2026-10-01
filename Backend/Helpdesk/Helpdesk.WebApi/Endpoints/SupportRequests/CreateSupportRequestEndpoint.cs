using FastEndpoints;
using Helpdesk.Services.RequestModels;
using Helpdesk.Services.ResponseModels;
using Helpdesk.Services.ResponseModels.Abstract;
using Helpdesk.Services.SupportRequests;

namespace Helpdesk.WebApi.Endpoints.SupportRequests;

public class CreateSupportRequestEndpoint : Endpoint<CreateSupportRequestModel, IDataResult<SupportRequestResponseModel>>
{
  private readonly ISupportRequestService _supportRequestService;

  public CreateSupportRequestEndpoint(ISupportRequestService supportRequestService)
  {
    _supportRequestService = supportRequestService;
  }

  public override void Configure()
  {
    Post("api/support-requests");
    Roles("User", "Admin");
  }

  public override async Task HandleAsync(CreateSupportRequestModel req, CancellationToken ct)
  {
    var result = await _supportRequestService.CreateAsync(req, ct);
    await Send.ResponseAsync(result, 200, ct);
  }
}
