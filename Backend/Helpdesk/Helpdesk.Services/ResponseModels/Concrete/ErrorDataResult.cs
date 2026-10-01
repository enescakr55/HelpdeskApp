using Helpdesk.Services.ResponseModels.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Services.ResponseModels.Concrete
{
  public class ErrorDataResult<T> : DataResult<T>, IDataResult<T>,IResult
  {
    public ErrorDataResult() : base(false)
    {

    }
    public ErrorDataResult(string message) : base(false, message)
    {

    }
    public ErrorDataResult(T data) : base(false, data)
    {

    }
    public ErrorDataResult(string message, T data) : base(false, data, message)
    {

    }
  }
}
