using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;

namespace Afrodite.Connection
{
    class TcpMaster
    {
        private TcpClient client;

        public TcpMaster()
        {
           
        }
    }

    class Pinger
    {
        private UdpClient udpClient;

        public Pinger(int pingerPort)
        {
            udpClient = new UdpClient(pingerPort);
        }

        public bool Ping(IPEndPoint endPoint)
        {
            udpClient.Send(new byte[1], 1, endPoint);
           var data = udpClient.Receive(ref endPoint);

            return true;
        }

        public bool Ping(string remoteIp, int remotePort)
        {
            throw new NotImplementedException();
        }
    }
}
