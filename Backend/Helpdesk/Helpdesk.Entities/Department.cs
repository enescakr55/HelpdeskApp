using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Entities
{
  public class Department
  {
    public string Id { get; set; }
    public string DepartmentName { get; set; }

    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<SupportRequest> SupportRequests { get; set; } = new List<SupportRequest>();
  }
}
