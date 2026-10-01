namespace Helpdesk.Services.RequestModels;

public class AddSupportRequestMessageModel
{
  public string Message { get; set; } = string.Empty;
  public bool IsUserMessage { get; set; }
}
