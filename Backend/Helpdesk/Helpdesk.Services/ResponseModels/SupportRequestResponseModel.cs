using Helpdesk.Entities.Flags;

namespace Helpdesk.Services.ResponseModels;

public class SupportRequestResponseModel
{
  public string Id { get; set; } = string.Empty;
  public string RequestCode { get; set; } = string.Empty;
  public string Fullname { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public int Subject { get; set; }
  public int Priority { get; set; }
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public SupportTypeStatusEnum Status { get; set; }
  public string? AssignedDepartmentId { get; set; }
  public string? AssignMessage { get; set; }
  public string? UserMessage { get; set; }
  public string AssignedDepartmentName { get; set; } = string.Empty;
}
