using System.Collections.Generic;

namespace Afrodite.Abstract
{
    public interface IPerformanceManager
    {
        Dictionary<string,float> GetCpusUsage();
        ulong GetAviableMemory();
        ulong GetTotalMemory();
        uint GetCpuCount();
        string GetMachineName();
        ulong GetTotalDiskSpace();
        void Dispose();
        ulong GetUsedMemory();
        float GetAvgCpusUsage();
    }
}