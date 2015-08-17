using System;
using Apache.NMS;
using Apache.NMS.ActiveMQ.Commands;
using System.Diagnostics;

namespace Afrodite
{
	public class ServerConnectionProvider : IDisposable
	{
		IMessageProducer producer;
		IMessageConsumer consument;
		ISession session;
		IConnection connection;

		public ServerConnectionProvider (string connectionUri)
		{
			var uri = new Uri (connectionUri);
			IConnectionFactory factory = new NMSConnectionFactory (uri);
			connection = factory.CreateConnection ();
			session = connection.CreateSession ();
			IDestination destination = session.GetDestination("queue://FOO.BAR");
			producer = session.CreateProducer ();

			consument = session.CreateConsumer (destination);
			consument.Listener += ConsumentListener;
			connection.Start ();
		}

		void ConsumentListener (IMessage message)
		{
			MessageType type = (MessageType)Enum.Parse (typeof(MessageType), message.NMSType, true);
			switch (type)
			{
			case MessageType.ConnectionRequest:
				ProcessConnectMsg (message);
				AckownlageMessage (message);
				break;
			case MessageType.Status:
				ProcessStatusMsg (message);
				break;
			}
		}

		void AckownlageMessage (IMessage message)
		{
			var ack = producer.CreateMessage ();
			ack.NMSType = "ACK";
			producer.Send (message.NMSReplyTo, ack);
		}

		public void ProcessConnectMsg (IMessage message)
		{

		}

		public void ProcessStatusMsg (IMessage message)
		{

		}

		public void Dispose ()
		{
			consument.Dispose ();
			producer.Dispose ();
			session.Dispose ();
			connection.Dispose ();
		}
	}
}

