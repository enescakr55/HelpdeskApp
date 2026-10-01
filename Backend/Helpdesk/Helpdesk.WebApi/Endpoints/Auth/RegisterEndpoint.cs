using FastEndpoints;
using Helpdesk.Services.Authentication;
using Helpdesk.Services.RequestModels;
using Helpdesk.Services.ResponseModels;
using Helpdesk.Services.ResponseModels.Abstract;

namespace Helpdesk.WebApi.Endpoints.Auth;

public class RegisterEndpoint : Endpoint<RegisterModel, IDataResult<AuthResponseModel>>
{
  private readonly IAuthService _authService;

  public RegisterEndpoint(IAuthService authService)
  {
    _authService = authService;
  }

  public override void Configure()
  {
    Post("api/auth/register");
    AllowAnonymous();
  }

  public override async Task HandleAsync(RegisterModel req, CancellationToken ct)
  {
    var result = await _authService.RegisterAsync(req, ct);
    await Send.ResponseAsync(result, 200, ct);
  }
}
