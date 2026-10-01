using FastEndpoints;
using Helpdesk.Services.Authentication;
using Helpdesk.Services.RequestModels;
using Helpdesk.Services.ResponseModels;
using Helpdesk.Services.ResponseModels.Abstract;

namespace Helpdesk.WebApi.Endpoints.Auth;

public class LoginEndpoint : Endpoint<LoginModel, IDataResult<AuthResponseModel>>
{
  private readonly IAuthService _authService;

  public LoginEndpoint(IAuthService authService)
  {
    _authService = authService;
  }

  public override void Configure()
  {
    Post("api/auth/login");
    AllowAnonymous();
  }

  public override async Task HandleAsync(LoginModel req, CancellationToken ct)
  {
    var result = await _authService.LoginAsync(req, ct);
    await Send.ResponseAsync(result, result.Success ? 200 : 400, ct);
  }
}
