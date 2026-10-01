using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Entities
{
  public class User
  {
    public string Id { get; set; }
    public  string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string DepartmentId { get; set; }
    public bool IsAdmin { get; set; }

    public Department Department { get; set; }
  }
}
