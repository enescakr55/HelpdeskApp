namespace Helpdesk.Services.RequestModels;

public class CreateSupportRequestModel
{
  public string Fullname { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public int Subject { get; set; }
  public int Priority { get; set; }
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
}
