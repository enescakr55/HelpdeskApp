using Helpdesk.Entities.Flags;

namespace Helpdesk.Services.ResponseModels;

public class SupportRequestTrackResponseModel
{
  public string RequestCode { get; set; } = string.Empty;
  public int Subject { get; set; }
  public int Priority { get; set; }
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public SupportTypeStatusEnum Status { get; set; }
  public string? UserMessage { get; set; }
}