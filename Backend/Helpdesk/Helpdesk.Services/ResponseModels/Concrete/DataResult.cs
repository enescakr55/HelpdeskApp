using Helpdesk.Services.ResponseModels.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Services.ResponseModels.Concrete
{
  public class DataResult<T> : Result, IDataResult<T>, IResult
  {
    public DataResult(bool success) : base(success)
    {
    }

    public DataResult(bool success, string message) : base(success, message)
    {
    }
    public DataResult(bool success, T data) : base(success)
    {
      this.Data = data;
    }
    public DataResult(bool success, T data, string message) : base(success,message)
    {
      this.Data = data;
    }
    public T Data { get; }
  }
}
