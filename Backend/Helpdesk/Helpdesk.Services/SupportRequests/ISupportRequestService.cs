using Helpdesk.Services.RequestModels;
using Helpdesk.Services.ResponseModels;
using Helpdesk.Services.ResponseModels.Abstract;

namespace Helpdesk.Services.SupportRequests;

public interface ISupportRequestService
{
  Task<IDataResult<SupportRequestResponseModel>> CreateAsync(CreateSupportRequestModel model, CancellationToken cancellationToken = default);
  Task<IDataResult<List<SupportRequestResponseModel>>> GetByUserAsync(string email, CancellationToken cancellationToken = default);
  Task<IDataResult<List<SupportRequestResponseModel>>> GetAllForAdminAsync(CancellationToken cancellationToken = default);
  Task<IDataResult<SupportRequestResponseModel>> UpdateStatusAsync(string id, UpdateSupportRequestStatusModel model, CancellationToken cancellationToken = default);
  Task<IDataResult<SupportRequestResponseModel>> AddMessageAsync(string id, AddSupportRequestMessageModel model, CancellationToken cancellationToken = default);
  Task<IDataResult<SupportRequestResponseModel>> AssignDepartmentAsync(string id, string departmentId, CancellationToken cancellationToken = default);
}
