using Helpdesk.Services.ResponseModels.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Services.ResponseModels.Concrete
{
  public class ErrorResult : Result,IResult
  {
    public ErrorResult() : base(false)
    {

    }
    public ErrorResult(string message) : base(false, message)
    {

    }
  }
}
