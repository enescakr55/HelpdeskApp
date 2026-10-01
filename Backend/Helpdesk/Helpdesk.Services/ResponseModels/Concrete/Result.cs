using Helpdesk.Services.ResponseModels.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Services.ResponseModels.Concrete
{
  public class Result : IResult
  {
    public Result(bool success)
    {
      this.Success = success;
    }
    public Result(bool success, string message) : this(success)
    {
      this.Message = message;
    }
    public bool Success { get; }
    public string Message { get; set; }
  }
}
