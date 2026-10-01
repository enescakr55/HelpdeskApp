using FastEndpoints;
using Helpdesk.Services.ResponseModels.Abstract;
using Helpdesk.Services.ResponseModels.Concrete;
using Helpdesk.Services.SupportRequests;
using IResult = Helpdesk.Services.ResponseModels.Abstract.IResult;

namespace Helpdesk.WebApi.Endpoints.SupportRequests
{
  public class DeleteSupportRequestEndpoint : EndpointWithoutRequest<IResult>
  {
    private readonly ISupportRequestService _supportRequestService;
    public DeleteSupportRequestEndpoint(ISupportRequestService supportRequestService)
    {
      _supportRequestService = supportRequestService;
    }
    public override void Configure()
    {
      Delete("api/support-requests/{id}/delete");
      Roles("Admin");
    }
    public override async Task HandleAsync(CancellationToken ct)
    {
      var requestId = Route<string>("id");
      if(requestId == null){
        await Send.ResponseAsync(new ErrorResult("Id boş olamaz"), 400);
        return;
      }
      var result = await _supportRequestService.DeleteSupportRequestAsync(requestId,ct);
      await Send.ResponseAsync(result, result.Success ? 200 : 400, ct);
    }
  }
}
