using System;

namespace HdProduction.Dashboard.Domain.Exceptions
{
  public class BusinessLogicException : ArgumentException
  {
    public BusinessLogicException(string message) : base(message)
    {
    }

    public BusinessLogicException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}