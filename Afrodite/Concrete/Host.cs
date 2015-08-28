using System;
using Afrodite.Abstract;

namespace Afrodite.Concrete
{
    public class Host : IHost
    {
        public int MachineNumber { get; set; }
        public Guid MachineId { get; set; }
        public int PingerPort { get; set; }
        public string IpOrHostname { get; set; }
    }
}