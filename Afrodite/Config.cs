using System;

namespace Afrodite
{
    public class Config : IConfig
    {
        public Guid MachineId { get; set; }
        public int Priority { get; set; }
    }
}