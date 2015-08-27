using System;

namespace Afrodite
{
    public class Config : RemoteHost
    {
        public int RecieveTimeout { get; set; }
	    public RemoteHost[] RemoteHosts { get; set; }
    }

    public class RemoteHost
    {
        public Guid MachineId { get; set; }
        public int Priority { get; set; }
        public int PingerPort { get; set; }
        public int DataTransportPort { get; set; }
    }
}