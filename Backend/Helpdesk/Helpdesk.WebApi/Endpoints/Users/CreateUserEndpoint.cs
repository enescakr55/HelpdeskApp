using FastEndpoints;
using Helpdesk.Services.RequestModels;
using Helpdesk.Services.ResponseModels;
using Helpdesk.Services.ResponseModels.Abstract;
using Helpdesk.Services.Users;

namespace Helpdesk.WebApi.Endpoints.Users
{
  public class CreateUserEndpoint : Endpoint<CreateUserModel,IDataResult<UserResponseModel>>
  {
    private readonly IUserService _userService;
    public CreateUserEndpoint(IUserService userService)
    {
      _userService = userService;
    }
    public override void Configure()
    {
      Post("api/users/create");
      AllowAnonymous();
    }
    public override async Task HandleAsync(CreateUserModel req, CancellationToken ct)
    {
      var result = await _userService.CreateAsync(req); 
      await Send.ResponseAsync(result,200);
    }
  }
}
