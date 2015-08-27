using System;

namespace Afrodite
{
    public interface IConfig
    {
        // string HostNameOrIp { get; set; }
        Guid MachineId { get; set; }
        int Priority { get; set; }
    }
}