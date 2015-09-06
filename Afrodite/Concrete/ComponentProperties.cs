using System;
using Afrodite.Abstract;

namespace Afrodite.Concrete
{
    public class ComponentProperties : IComponentProperties
    {
        public string CpuInfo { get; set; }
        public uint CpuCoresCount { get; set; }
        public ulong TotalMemoryMBytes { get; set; }
        public string OsInfo { get; set; }
        public ulong TotalDiskMBytes { get; set; }
        public string CoputerName { get; set; }
        public string HostName { get; set; }
        public Guid MachineId { get; set; }
		public int MachineNumber { get; set; }
    }
}