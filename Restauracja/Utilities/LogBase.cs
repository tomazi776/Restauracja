using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restauracja.Utilities
{
    public abstract class LogBase
    {
        public abstract void Log(Exception exception);
        public virtual string BuildLogs(Exception ex)
        {
            StringBuilder exceptionBuilder = new StringBuilder();
            exceptionBuilder.Append("Exception Type: ")
                .Append(Environment.NewLine)
                .Append(ex.GetType().Name)
                .Append(Environment.NewLine)
                .Append(Environment.NewLine)
                .Append("Message: ")
                .Append(Environment.NewLine)
                .Append(ex.Message)
                .Append(Environment.NewLine)
                .Append(Environment.NewLine)
                .Append("StackTrace: ")
                .Append(Environment.NewLine)
                .Append(ex.StackTrace)
                .Append(Environment.NewLine)
                .Append(Environment.NewLine);

            Exception innerException = ex.InnerException;

            while (innerException != null)
            {
                exceptionBuilder.Append("Inner Exception Type: ")
                    .Append(Environment.NewLine)
                    .Append(innerException.GetType().Name)
                    .Append(Environment.NewLine)
                    .Append(Environment.NewLine)
                    .Append("Message: ")
                    .Append(Environment.NewLine)
                    .Append(innerException.Message)
                    .Append(Environment.NewLine)
                    .Append(Environment.NewLine)
                    .Append("StackTrace: ")
                    .Append(Environment.NewLine)
                    .Append(innerException.StackTrace)
                    .Append(Environment.NewLine)
                    .Append(Environment.NewLine);

                innerException = innerException.InnerException;
            }
            return exceptionBuilder.ToString();
        }
    }
}
