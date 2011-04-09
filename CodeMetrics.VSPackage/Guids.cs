// Guids.cs
// MUST match guids.h
using System;

namespace CodeMetrics.CodeMetrics_VSPackage
{
    static class GuidList
    {
        public const string guidCodeMetrics_VSPackagePkgString = "f88105ce-5c92-4666-9126-f3a54916cb8d";
        public const string guidCodeMetrics_VSPackageCmdSetString = "13c9aed8-71f1-46b1-a707-9896bbe03403";

        public static readonly Guid guidCodeMetrics_VSPackageCmdSet = new Guid(guidCodeMetrics_VSPackageCmdSetString);
    };
}