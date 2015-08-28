using System;

namespace Afrodite
{
    public class Config : RemoteHost
    {
	    public RemoteHost[] RemoteHosts { get; set; }
    }

    public class RemoteHost
    {
        public int MachineNumber { get; set; }
        public Guid MachineId { get; set; }
        public int PingerPort { get; set; }
        public string MachineIp { get; set; }
    }
}