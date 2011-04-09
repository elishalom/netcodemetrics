// Guids.cs
// MUST match guids.h
using System;

namespace EliShalom.CodeMetrics_Addin
{
    static class GuidList
    {
        public const string guidCodeMetrics_AddinPkgString = "010275b7-93f0-49b4-a2d8-6a1eda131e57";
        public const string guidCodeMetrics_AddinCmdSetString = "7d4c2edd-0f52-4db5-a45e-a173bdbc051a";

        public static readonly Guid guidCodeMetrics_AddinCmdSet = new Guid(guidCodeMetrics_AddinCmdSetString);
    };
}