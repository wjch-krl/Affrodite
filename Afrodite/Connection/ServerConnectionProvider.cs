using System;
using Apache.NMS;
using Apache.NMS.ActiveMQ.Commands;
using System.Diagnostics;
using Apache.NMS.ActiveMQ.Threads;

namespace Afrodite
{
	public class ServerConnectionProvider : IDisposable
	{
		IMessageProducer producer;
		IMessageConsumer consument;
		ISession session;
		IConnection connection;
		IMachinesManager machineManager;
		IStatesManager machineStates;

		public ServerConnectionProvider (NMSConnectionFactory factory,string masterQueueName,
			IMachinesManager machineManager,IStatesManager machineStates)
		{
			this.machineStates = machineStates;
			this.machineManager = machineManager;
			connection = factory.CreateConnection ();
			session = connection.CreateSession ();
			IDestination destination = session.GetDestination(masterQueueName);
			producer = session.CreateProducer ();

			consument = session.CreateConsumer (destination);
			consument.Listener += ConsumentListener;
			connection.Start ();
		}

		private void ConsumentListener (IMessage message)
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

		private void AckownlageMessage (IMessage message)
		{
			var ack = producer.CreateMessage ();
			ack.NMSType = MessageType.Affirmation.ToString ();
			producer.Send (message.NMSReplyTo, ack);
		}

		private void ProcessConnectMsg (IMessage message)
		{
			var props = message.ToObject<IComponentProperties> ();
			machineManager.Add (props);
		}

		private void ProcessStatusMsg (IMessage message)
		{
			var state = message.ToObject<IComponentState> ();
			machineStates [state.MachineId].Add (state);
		}

		public bool StartNewJob<T>(IMachineJob<T> job, int machineId)
		{
			throw new NotImplementedException ();
		}

		public bool StopJob(Guid jobId,int machineId)
		{
			throw new NotImplementedException ();

		}

		public bool PauseJob(Guid jobId,int machineId)
		{
			throw new NotImplementedException ();

		}

		public bool ResumeJob(Guid jobId,int machineId)
		{
			throw new NotImplementedException ();
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

