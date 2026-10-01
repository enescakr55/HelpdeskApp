using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Services.RequestModels
{
  public class LoginResponseModel
  {
    public DateTime Expiration { get; set; }
    public string Token { get; set; }
  }
}
