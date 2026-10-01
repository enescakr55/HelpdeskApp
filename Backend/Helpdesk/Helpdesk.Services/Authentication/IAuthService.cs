using Helpdesk.Services.RequestModels;
using Helpdesk.Services.ResponseModels;
using Helpdesk.Services.ResponseModels.Abstract;

namespace Helpdesk.Services.Authentication;

public interface IAuthService
{
  Task<IDataResult<AuthResponseModel>> RegisterAsync(RegisterModel model, CancellationToken cancellationToken = default);
  Task<IDataResult<AuthResponseModel>> LoginAsync(LoginModel model, CancellationToken cancellationToken = default);
}
