// Guids.cs
// MUST match guids.h
using System;

namespace CodeMetrics.VSPackage2
{
    static class GuidList
    {
        public const string guidVSPackage2PkgString = "66362c94-6c46-4796-ad53-0a154c8b4245";
        public const string guidVSPackage2CmdSetString = "39a24871-d8e0-4b19-a08b-4128eff31071";

        public static readonly Guid guidVSPackage2CmdSet = new Guid(guidVSPackage2CmdSetString);
    };
}