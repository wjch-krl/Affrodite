using Afrodite.Abstract;

namespace Afrodite.Concrete
{
    public class ComponentProperties : IComponentProperties
    {
        public string CpuInfo { get; set; }
        public int CpuCoresCount { get; set; }
        public int TotalMemoryMBytes { get; set; }
        public string OsInfo { get; set; }
        public int TotalDiskMBytes { get; set; }
        public string CoputerName { get; set; }
        public string HostName { get; set; }
        public int MachineId { get; set; }
    }
}