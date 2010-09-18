using CodeMetrics.DataTransferObjects;

namespace CodeMetrics.WorkerServices
{
    public class CodeMetricsService : ICodeMetricsService
    {
        public TaskResult PerformTask(TaskRequest task)
        {
            return new TaskResult()
                {
                    ID = task.ID,
                    ComplexityValue = 2
                };
        }
    }
}