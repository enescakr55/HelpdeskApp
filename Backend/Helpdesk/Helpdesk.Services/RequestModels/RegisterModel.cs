namespace Helpdesk.Services.RequestModels;

public class RegisterModel
{
  public string Firstname { get; set; } = string.Empty;
  public string Lastname { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
  public string DepartmentId { get; set; } = string.Empty;
}
