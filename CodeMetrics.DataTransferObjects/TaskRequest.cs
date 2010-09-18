using System;
using System.Runtime.Serialization;

namespace CodeMetrics.DataTransferObjects
{
    [DataContract]
    public class TaskRequest
    {
        [DataMember]
        public Guid ID { get; set; }

        [DataMember]
        public string MethodBody { get; set; }
    }

    [DataContract]
    public class TaskResult
    {
        [DataMember]
        public Guid ID { get; set; }

        [DataMember]
        public int ComplexityValue { get; set; }
    }
}
