using Helpdesk.Entities.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Entities
{
  public class SupportRequest
  {
    public string Id { get; set; }
    public string RequestCode { get; set; }
    public string Fullname { get; set; }
    public string Email { get; set; }
    public int Subject { get; set; }
    public int Priority { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public SupportTypeStatusEnum Status { get; set; }
    public string? AssignedDepartmentId { get; set; }
    public string? AssignMessage { get; set; }
    public string? UserMessage { get; set; }

    public Department? AssignedDepartment { get; set; }
  }
}
