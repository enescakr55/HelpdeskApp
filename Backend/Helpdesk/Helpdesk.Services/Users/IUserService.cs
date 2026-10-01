using Helpdesk.Services.ResponseModels;
using Helpdesk.Services.ResponseModels.Abstract;
using Helpdesk.Services.RequestModels;

namespace Helpdesk.Services.Users;

public interface IUserService
{
  Task<IDataResult<UserResponseModel>> CreateAsync(
    CreateUserModel model,
    CancellationToken cancellationToken = default);
}