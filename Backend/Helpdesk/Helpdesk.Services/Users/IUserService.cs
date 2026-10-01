using Helpdesk.Services.ResponseModels;
using Helpdesk.Services.ResponseModels.Abstract;
using Helpdesk.Services.RequestModels;

namespace Helpdesk.Services.Users;

public interface IUserService
{
  Task<IDataResult<List<UserResponseModel>>> ListAsync(
    CancellationToken cancellationToken = default);

  Task<IDataResult<List<UserResponseModel>>> ListPendingManagersAsync(
    CancellationToken cancellationToken = default);

  Task<IDataResult<UserResponseModel>> ApproveManagerAsync(
    string userId,
    CancellationToken cancellationToken = default);

  Task<IDataResult<UserResponseModel>> CreateAsync(
    CreateUserModel model,
    CancellationToken cancellationToken = default);
}