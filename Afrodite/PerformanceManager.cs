using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic.Devices;

namespace Afrodite
{
    internal class PerformanceManager : IDisposable
    {
        private PerformanceCounter performanceCounter;

        public static float Calculate(CounterSample oldSample, CounterSample newSample)
        {
            double difference = newSample.RawValue - oldSample.RawValue;
            double timeInterval = newSample.TimeStamp100nSec - oldSample.TimeStamp100nSec;
            return (float) (100*(1 - (difference/timeInterval)));
        }

        public Dictionary<string,float> GetCpusUsage()
        {
            performanceCounter = new PerformanceCounter("Processor Information", "% Processor Time");
            var counterCategory = new PerformanceCounterCategory("Processor Information");
            var instances = counterCategory.GetInstanceNames();
            var samples = instances.ToDictionary(x=>x,y=>new List<CounterSample>());

            for (int i = 0; i < 2; i++)
            {
                foreach (var instanceName in instances)
                {
                    performanceCounter.InstanceName = instanceName;
                    samples[instanceName].Add(performanceCounter.NextSample());
                }
            }
            return samples.ToDictionary(x => x.Key, y => Calculate(y.Value[0], y.Value[1]));
        }

        public ulong GetAviableMemory()
        {
            return new ComputerInfo().AvailablePhysicalMemory;
        }

        public ulong GetTotalMemory()
        {
            return new ComputerInfo().TotalPhysicalMemory;
        }

        public uint GetCpuCount()
        {
            return (uint)Environment.ProcessorCount;
        }

        public string GetMachineName()
        {
            return Environment.MachineName;
        }

        public string GetMachineIp()
        {
            //Z configu wyciągnąc
           throw new NotImplementedException();
        }

        public ulong GetTotalDiskSpace()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            return (ulong)drives.Sum(x => x.TotalSize);
        }

        public void Dispose()
        {
            performanceCounter.Dispose();
        }
    }
}