using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Afrodite.Common;

namespace Afrodite.Connection
{
	public class Pinger : IDisposable
	{
		private readonly int pingerPort;
		private UdpClient udpClient;
		private static readonly byte[] EhoMsg = { 1, 2, 3, 4, 5, 6, 7 };
		private static readonly byte[] EhoReplyMsg = { 7, 6, 5, 4, 3, 2, 1 };
		private bool run;
		private Dictionary<string, byte[]> echoRpelies;

		public Pinger (int pingerPort, int timeout)
		{
			this.pingerPort = pingerPort;
			run = true;
			udpClient = new UdpClient (pingerPort) { Client = { ReceiveTimeout = timeout } };
			echoRpelies = new Dictionary<string, byte[]> ();
			Task.Factory.StartNew (RecieveUdp);
		}

		private void RecieveUdp ()
		{
			do
			{
				IPEndPoint reciveEndPoint = new IPEndPoint (IPAddress.Any, pingerPort);
				try
				{
					ListenForDatagrams (reciveEndPoint);
				}
				catch (Exception ex)
				{
					Logger.LoggError (ex, Logger.GetCurrentMethod ());
				}
				lock (echoRpelies)
				{
					Monitor.PulseAll (echoRpelies);
				}
			} while (run);
		}

		private void ListenForDatagrams (IPEndPoint reciveEndPoint)
		{
			var data = udpClient.Receive (ref reciveEndPoint);
			if (data.SequenceEqual (EhoMsg))
			{
				udpClient.Send (EhoReplyMsg, EhoReplyMsg.Length, reciveEndPoint);
			}
			else if (data.SequenceEqual (EhoReplyMsg))
			{
				lock (echoRpelies)
				{
					var ipStr = reciveEndPoint.Address.ToString ();
					if (echoRpelies.ContainsKey (ipStr))
						return;
					echoRpelies.Add (ipStr, data);
				}
			}
		}

		public bool Ping (IPEndPoint endPoint)
		{
			try
			{
				udpClient.Send (EhoMsg, EhoMsg.Length, endPoint);
				var data = RecieveEcho (endPoint.Address.ToString ());
				return data != null && data.SequenceEqual (EhoReplyMsg);
			}
			catch (Exception ex)
			{
				Logger.LoggError (ex, Logger.GetCurrentMethod ());
				return false;
			}
		}

		private byte[] RecieveEcho (string ip)
		{
            
			int retries = 0;
			do
			{
				lock (echoRpelies)
				{
					Monitor.Wait (echoRpelies);
					if (echoRpelies.ContainsKey (ip))
					{
						var data = echoRpelies [ip];
						echoRpelies.Remove (ip);
						return data;
					}
				}
			} while (retries++ < 3);
			return null;
		}

		public bool Ping (string remoteIp, int remotePort)
		{
			IPAddress ip = IPAddress.Parse (remoteIp);
			return Ping (new IPEndPoint (ip, remotePort));
		}

		public void Dispose ()
		{
			run = false;
		}
	}
}