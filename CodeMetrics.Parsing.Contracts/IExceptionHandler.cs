using System;

namespace CodeMetrics.Parsing.Contracts
{
    public interface IExceptionHandler
    {
        void HandleException(Exception exception);
    }
}