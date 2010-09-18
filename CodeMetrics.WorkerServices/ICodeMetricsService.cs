using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CodeMetrics.DataTransferObjects;

namespace CodeMetrics.WorkerServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICodeMetricsService" in both code and config file together.
    [ServiceContract]
    public interface ICodeMetricsService
    {
        [OperationContract]
        TaskResult PerformTask(TaskRequest task);
    }
}
