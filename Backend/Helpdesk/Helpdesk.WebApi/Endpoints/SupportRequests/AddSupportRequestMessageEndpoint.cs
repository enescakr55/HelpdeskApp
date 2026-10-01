using FastEndpoints;
using Helpdesk.Services.RequestModels;
using Helpdesk.Services.ResponseModels;
using Helpdesk.Services.ResponseModels.Abstract;
using Helpdesk.Services.SupportRequests;

namespace Helpdesk.WebApi.Endpoints.SupportRequests;

public class AddSupportRequestMessageEndpoint : Endpoint<AddSupportRequestMessageModel, IDataResult<SupportRequestResponseModel>>
{
  private readonly ISupportRequestService _supportRequestService;

  public AddSupportRequestMessageEndpoint(ISupportRequestService supportRequestService)
  {
    _supportRequestService = supportRequestService;
  }

  public override void Configure()
  {
    Post("api/support-requests/{id}/message");
    Roles("User", "Admin");
  }

  public override async Task HandleAsync(AddSupportRequestMessageModel req, CancellationToken ct)
  {
    var id = Route<string>("id");
    var result = await _supportRequestService.AddMessageAsync(id, req, ct);
    await Send.ResponseAsync(result, result.Success ? 200 : 400, ct);
  }
}
