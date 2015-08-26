using System;
using System.Threading;
using System.Threading.Tasks;
using Afrodite.Abstract;
using Apache.NMS;

namespace Afrodite.Connection
{
	public class ClientConnectionProvider : IDisposable
	{
		IMessageProducer producer;
		IMessageConsumer consument;
		ISession session;
		IConnection connection;
		IComponentPropertiesBuilder propertiesBuilder;
		IDestination masterQueue;
		ICurrentMachineStateManager componentStateManager;
		bool run;
		int stateInnterval;

		public ClientConnectionProvider (NMSConnectionFactory factory, int timeout, string 
			masterQueueName, string clientQueueName, IComponentPropertiesBuilder propertiesBuilder,
			ICurrentMachineStateManager componentStateManager, int stateInnterval)
		{
			this.stateInnterval = stateInnterval;
			this.componentStateManager = componentStateManager;
			this.propertiesBuilder = propertiesBuilder;
	
			connection = factory.CreateConnection ();
			session = connection.CreateSession ();
			masterQueue = session.GetDestination (masterQueueName);

			ConnectToMaster (masterQueue,timeout);

			consument = session.CreateConsumer (session.GetQueue (clientQueueName));
			consument.Listener += ConsumentListener;

			connection.Start ();
			run = true;
			Task.Factory.StartNew (StartSendingState);
		}

		void StartSendingState()
		{
			do
			{
				SendState ();
				Thread.Sleep (stateInnterval);

			} while (run);
		}

		void SendState ()
		{
			using (var clientProd = session.CreateProducer ())
			{
				var state = componentStateManager.CurrentState;
				var msg = session.CreateObjectMessage (state);
				msg.NMSType = MessageType.Status.ToString ();
				clientProd.Send (msg);
			}
		}

		void ConnectToMaster (IDestination masterQueue, int timeout)
		{
			producer = session.CreateProducer ();
			var props = propertiesBuilder.CreateProperties ();
			using (var clientProd = session.CreateProducer ())
			{
				using (var clientCons = session.CreateConsumer (masterQueue))
				{
					var msg = session.CreateObjectMessage (props);
					msg.NMSType = MessageType.ConnectionRequest.ToString ();
					clientProd.Send (msg);
					var ack = clientCons.Receive (TimeSpan.FromMilliseconds (timeout));
					if (ack == null)
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

