using FastEndpoints;
using Helpdesk.Services.RequestModels;
using Helpdesk.Services.ResponseModels;
using Helpdesk.Services.ResponseModels.Abstract;
using Helpdesk.Services.Users;

namespace Helpdesk.WebApi.Endpoints.Users;

public class ApproveManagerEndpoint : Endpoint<ApproveManagerModel, IDataResult<UserResponseModel>>
{
  private readonly IUserService _userService;

  public ApproveManagerEndpoint(IUserService userService)
  {
    _userService = userService;
  }

  public override void Configure()
  {
    Post("api/users/approve-manager");
    Roles("Admin");
  }

  public override async Task HandleAsync(ApproveManagerModel req, CancellationToken ct)
  {
    var result = await _userService.ApproveManagerAsync(req.UserId, ct);
    await Send.ResponseAsync(result, result.Success ? 200 : 400, ct);
  }
}