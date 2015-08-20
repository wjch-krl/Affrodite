using System;
using Apache.NMS;

namespace Afrodite
{
	public class ClientConnectionProvider : IDisposable
	{
		IMessageProducer producer;
		IMessageConsumer consument;
		ISession session;
		IConnection connection;
		IComponentPropertiesBuilder propertiesBuilder;

		public ClientConnectionProvider (NMSConnectionFactory factory, int timeout,
		                                 string masterQueueName, string clientQueueName, IComponentPropertiesBuilder propertiesBuilder)
		{
			connection = factory.CreateConnection ();
			session = connection.CreateSession ();
			ConnectToMaster (masterQueueName,timeout);

			this.propertiesBuilder = propertiesBuilder;
			consument = session.CreateConsumer (session.GetQueue (clientQueueName));
			consument.Listener += ConsumentListener;
			connection.Start ();
		}

		void ConnectToMaster (string masterQueueName, int timeout)
		{
			IDestination destination = session.GetDestination (masterQueueName);
			producer = session.CreateProducer ();
			var props = propertiesBuilder.CreateProperties ();
			using (var clientProd = session.CreateProducer ())
			{
				using (var clientCons = session.CreateConsumer (destination))
				{
					clientProd.Send (session.CreateObjectMessage (props));
					var msg = clientCons.Receive (TimeSpan.FromMilliseconds (timeout));
					if (msg == null)
					{
						throw new TimeoutException ("Master not answering");
					}
					if (msg.NMSType != MessageType.Affirmation.ToString ())
					{
						throw new InvalidOperationException ("Master didn't ACK");
					}
				}
			}
		}

		private void ConsumentListener (IMessage message)
		{
			MessageType type = (MessageType)Enum.Parse (
				                   typeof(MessageType), message.NMSType, true);
			switch (type)
			{
			case MessageType.PauseJob:
				ProcessPause (message);
				AckownlageMessage (message);
				break;
			case MessageType.ResumeJob:
				ProcessResume (message);
				AckownlageMessage (message);
				break;
			case MessageType.StartNewJob:
				ProcessStart (message);
				AckownlageMessage (message);
				break;
			case MessageType.StopJob:
				ProcessStop (message);
				AckownlageMessage (message);
				break;
			}
		}

		private void ProcessPause (IMessage message)
		{
			throw new NotImplementedException ();
		}

		private void ProcessResume (IMessage message)
		{
			throw new NotImplementedException ();

		}

		private void ProcessStart (IMessage message)
		{
			throw new NotImplementedException ();

		}

		private void ProcessStop (IMessage message)
		{
			throw new NotImplementedException ();

		}

		private void AckownlageMessage (IMessage message)
		{
			var ack = producer.CreateMessage ();
			ack.NMSType = MessageType.Affirmation.ToString ();
			producer.Send (message.NMSReplyTo, ack);
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

