using System.Collections.Generic;
using Afrodite.Abstract;

namespace Afrodite.Connection
{
    public interface IRemoteMachinesManager
    {
        IList<IHost> AviableHosts { get; }
        void Dispose();
    }
}