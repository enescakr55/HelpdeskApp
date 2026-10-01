using Helpdesk.Services.RequestModels;
using Helpdesk.Services.ResponseModels;
using Helpdesk.Services.ResponseModels.Abstract;

namespace Helpdesk.Services.Departments;

public interface IDepartmentService
{
  Task<IDataResult<List<DepartmentResponseModel>>> ListAsync(CancellationToken cancellationToken = default);
  Task<IDataResult<DepartmentResponseModel>> CreateAsync(CreateDepartmentModel model, CancellationToken cancellationToken = default);
  Task<IDataResult<DepartmentResponseModel>> UpdateAsync(string id, UpdateDepartmentModel model, CancellationToken cancellationToken = default);
  Task<IDataResult<bool>> DeleteAsync(string id, CancellationToken cancellationToken = default);
}
