using System;

namespace HdProduction.Dashboard.Domain.Exceptions
{
    public class ValidationException : BusinessLogicException
    {
        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}