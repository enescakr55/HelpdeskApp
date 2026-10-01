namespace Helpdesk.Services.ResponseModels;

public class AuthResponseModel
{
  public string Token { get; set; } = string.Empty;
  public DateTime Expiration { get; set; }
  public string UserId { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public string Firstname { get; set; } = string.Empty;
  public string Lastname { get; set; } = string.Empty;
  public bool IsAdmin { get; set; }
  public bool IsActive { get; set; }
}
