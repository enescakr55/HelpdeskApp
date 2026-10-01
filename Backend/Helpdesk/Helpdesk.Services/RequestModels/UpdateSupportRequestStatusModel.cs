using Helpdesk.Entities.Flags;

namespace Helpdesk.Services.RequestModels;

public class UpdateSupportRequestStatusModel
{
  public SupportTypeStatusEnum Status { get; set; }
}
