using System;

namespace Afrodite.Abstract
{
    public interface IHost
    {
        int MachineNumber { get; set; }
        Guid MachineId { get; set; }
        int PingerPort { get; set; }
        string IpOrHostname { get; set; }
    }
}