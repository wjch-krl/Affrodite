using System;
using System.Net;
using System.Net.Sockets;

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
            throw new NotImplementedException();
        }

        public bool Ping(string remoteIp, int remotePort)
        {
            IPAddress ip = IPAddress.Parse(remoteIp);
            return Ping(new IPEndPoint(ip, remotePort));
        }
    }
}
