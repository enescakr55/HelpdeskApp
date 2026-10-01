namespace Helpdesk.Services.ResponseModels;

public class UserResponseModel
{
  public UserResponseModel()
  {
    
  }
  public UserResponseModel(string id, string firstname, string lastname, string email, string departmentId, bool isAdmin)
  {
    Id = id;
    Firstname = firstname;
    Lastname = lastname;
    Email = email;
    DepartmentId = departmentId;
    IsAdmin = isAdmin;
  }

  public string Id { get; set; }
  public string Firstname { get; set; }
  public string Lastname { get; set; }
  public string Email { get; set; }
  public string DepartmentId { get; set; }
  public bool IsAdmin { get; set; }
}
