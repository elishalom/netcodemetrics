using System;
using CodeMetrics.Parsing.Contracts;

namespace CodeMetrics.Parsing
{
    public class UnderlyingObject : IUnderlyingObject
    {
        public UnderlyingObject(object rawObject)
        {
            if (rawObject == null) throw new ArgumentNullException(nameof(rawObject));

            RawObject = rawObject;
        }

        public object RawObject { get; }
    }
}