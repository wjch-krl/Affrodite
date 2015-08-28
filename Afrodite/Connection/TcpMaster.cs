using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Afrodite.Common;
using Afrodite.Concrete;

namespace Afrodite.Connection
{
    internal class TcpMaster : IDisposable
    {
        private TcpListener listener;
        private bool run;
        private ISerializer serializer;

        public TcpMaster(int port, ISerializer serializer)
        {
            this.serializer = serializer;
            listener = new TcpListener(new IPEndPoint(IPAddress.Any, port));
            listener.Start();
            run = true;
            Task.Factory.StartNew(AcceptClients);
        }

        private void AcceptClients()
        {
            do
            {
                try
                {
                    var client = listener.AcceptTcpClient();
                    ProcessClient(client);
                }
                catch (Exception)
                {
                    //TODO handle
                }
            } while (run);
        }

        private void ProcessClient(TcpClient client)
        {
            var stream = client.GetStream();
            using (var reader = new StreamReader(stream))
            {
                string serializedMsg = reader.ReadToEnd();
                ComponentProperties props = serializer.Deserialize<ComponentProperties>(serializedMsg);

            }
        }

        public void Dispose()
        {
            run = false;
            listener.Stop();
        }
    }
}