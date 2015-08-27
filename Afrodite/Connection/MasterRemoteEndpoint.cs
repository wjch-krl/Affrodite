using System;
using Afrodite.Abstract;
using Afrodite.Connection.Abstract;
using Apache.NMS;

namespace Afrodite.Connection
{
    public class MasterRemoteEndpoint<T> : IDisposable
    {
        private readonly IMessageProducer producer;
        private readonly IMessageConsumer consument;
        private readonly ISession session;
        private readonly IConnection connection;
        public IMachinesManager MachineManager { get; private set; }
        private readonly TimeSpan recieveTimeout;
        public IStatesManager<T> MachineStates { get; private set; }

        public MasterRemoteEndpoint(NMSConnectionFactory factory, string masterQueueName, long recieveTimeout,
            IMachinesManager machineManager, IStatesManager<T> machineStates)
        {
            this.recieveTimeout = new TimeSpan(recieveTimeout);
            this.MachineStates = machineStates;
            this.MachineManager = machineManager;
            connection = factory.CreateConnection();
            session = connection.CreateSession();
            IDestination destination = session.GetDestination(masterQueueName);
            producer = session.CreateProducer();

            consument = session.CreateConsumer(destination);
            consument.Listener += ConsumentListener;
            connection.Start();
        }

        private void ConsumentListener(IMessage message)
        {
            MessageType type = (MessageType) Enum.Parse(typeof (MessageType), message.NMSType, true);
            switch (type)
            {
                case MessageType.ConnectionRequest:
                    ProcessConnectMsg(message);
                    AckownlageMessage(message);
                    break;
                case MessageType.Status:
                    ProcessStatusMsg(message);
                    break;
            }
        }

        private void AckownlageMessage(IMessage message)
        {
            var ack = producer.CreateMessage();
            ack.NMSType = MessageType.Affirmation.ToString();
            producer.Send(message.NMSReplyTo, ack);
        }

        private void ProcessConnectMsg(IMessage message)
        {
            var props = message.ToObject<IComponentProperties>();
            MachineManager.Add(props);
        }

        private void ProcessStatusMsg(IMessage message)
        {
            var state = message.ToObject<IComponentState<T>>();
            MachineStates[state.MachineId].Add(state);
        }

        public bool StartNewJob(IJob<T> job, Guid machineId)
        {
            return SendJobActionMessage(job, machineId, MessageType.StartNewJob);
        }

        public bool StopJob(Guid jobId, Guid machineId)
        {
            return SendJobActionMessage(jobId, machineId, MessageType.StopJob);
        }

        public bool PauseJob(Guid jobId, Guid machineId)
        {
            return SendJobActionMessage(jobId, machineId,MessageType.ResumeJob);
        }

        public bool ResumeJob(Guid jobId, Guid machineId)
        {
            return SendJobActionMessage(jobId, machineId, MessageType.ResumeJob);
        }

        private bool SendJobActionMessage<TArg>(TArg msgBody, Guid machineId, MessageType type)
        {
            using (IDestination destination = session.GetQueue(String.Format("{0}", machineId)))
            {
                using (var clientcons = session.CreateConsumer(destination))
                {
                    using (var clientProd = session.CreateProducer())
                    {
                        var msg = session.CreateObjectMessage(msgBody);
                        msg.NMSType = type.ToString();
                        clientProd.Send(destination, msg);
                    }
                    var ack = clientcons.Receive(recieveTimeout);
                    if (ack == null)
                    {
                        throw new TimeoutException("Client not answering");
                    }
                    return ack.IsAcknowledge();
                }
            }
        }

        public void Dispose()
        {
            consument.Dispose();
            producer.Dispose();
            session.Dispose();
            connection.Dispose();
        }
    }
}