using FastEndpoints;
using Helpdesk.Services.ResponseModels;
using Helpdesk.Services.ResponseModels.Abstract;
using Helpdesk.Services.Users;

namespace Helpdesk.WebApi.Endpoints.Users;

public class ListPendingManagersEndpoint : EndpointWithoutRequest<IDataResult<List<UserResponseModel>>>
{
  private readonly IUserService _userService;

  public ListPendingManagersEndpoint(IUserService userService)
  {
    _userService = userService;
  }

  public override void Configure()
  {
    Get("api/users/pending-approvals");
    Roles("Admin");
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    var result = await _userService.ListPendingManagersAsync(ct);
    await Send.ResponseAsync(result, result.Success ? 200 : 400, ct);
  }
}