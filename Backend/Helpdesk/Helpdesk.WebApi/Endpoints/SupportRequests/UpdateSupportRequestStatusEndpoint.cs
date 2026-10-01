using FastEndpoints;
using Helpdesk.Services.RequestModels;
using Helpdesk.Services.ResponseModels;
using Helpdesk.Services.ResponseModels.Abstract;
using Helpdesk.Services.SupportRequests;

namespace Helpdesk.WebApi.Endpoints.SupportRequests;

public class UpdateSupportRequestStatusEndpoint : Endpoint<UpdateSupportRequestStatusModel, IDataResult<SupportRequestResponseModel>>
{
  private readonly ISupportRequestService _supportRequestService;

  public UpdateSupportRequestStatusEndpoint(ISupportRequestService supportRequestService)
  {
    _supportRequestService = supportRequestService;
  }

  public override void Configure()
  {
    Post("api/support-requests/{id}/status");
    Roles("Admin");
  }

  public override async Task HandleAsync(UpdateSupportRequestStatusModel req, CancellationToken ct)
  {
    var id = Route<string>("id");
    var result = await _supportRequestService.UpdateStatusAsync(id, req, ct);
    await Send.ResponseAsync(result, 200, ct);
  }
}
