using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Afrodite.Connection
{
    internal class Pinger : IDisposable
    {
        private readonly int pingerPort;
        private UdpClient udpClient;
        private static readonly byte[] EhoMsg = {1, 2, 3, 4, 5, 6, 7};
        private static readonly byte[] EhoReplyMsg = {7, 6, 5, 4, 3, 2, 1};
        private bool run;
        private Dictionary<string, byte[]> echoRpelies;

        public Pinger(int pingerPort, int timeout)
        {
            this.pingerPort = pingerPort;
            run = true;
            udpClient = new UdpClient(pingerPort) {Client = {ReceiveTimeout = timeout}};
            echoRpelies = new Dictionary<string, byte[]>();
            Task.Factory.StartNew(RecieveUdp);
        }

        private void RecieveUdp()
        {
            do
            {
                IPEndPoint reciveEndPoint = new IPEndPoint(IPAddress.Any, pingerPort);
                try
                {
                    GetValue(reciveEndPoint);
                }
                catch (Exception ex)
                {
                    //TODO logg exception
                }
            } while (run);
        }

        private void GetValue(IPEndPoint reciveEndPoint)
        {
            var data = udpClient.Receive(ref reciveEndPoint);
            if (data.SequenceEqual(EhoMsg))
            {
                udpClient.Send(EhoReplyMsg, EhoReplyMsg.Length, reciveEndPoint);
            }
            else if (data.SequenceEqual(EhoReplyMsg))
            {
                lock (echoRpelies)
                {
                    var ipStr = reciveEndPoint.Address.ToString();
                    if (echoRpelies.ContainsKey(ipStr))
                        return;
                    echoRpelies.Add(ipStr, data);
                    Monitor.PulseAll(echoRpelies);
                }
            }
        }

        public bool Ping(IPEndPoint endPoint)
        {
            try
            {
                udpClient.Send(EhoMsg, EhoMsg.Length, endPoint);
                var data = RecieveEcho(endPoint.Address.ToString());
                return data.SequenceEqual(EhoReplyMsg);
            }
            catch (Exception ex)
            {
                //TODO logg exception
                return false;
            }
        }

        private byte[] RecieveEcho(string ip)
        {
            lock (echoRpelies)
            {
                do
                {
                    Monitor.Wait(echoRpelies);
                    if (echoRpelies.ContainsKey(ip))
                    {
                        var data = echoRpelies[ip];
                        echoRpelies.Remove(ip);
                        return data;
                    }
                } while (true);
            }
        }

        public bool Ping(string remoteIp, int remotePort)
        {
            IPAddress ip = IPAddress.Parse(remoteIp);
            return Ping(new IPEndPoint(ip, remotePort));
        }

        public void Dispose()
        {
            run = false;
        }
    }
}