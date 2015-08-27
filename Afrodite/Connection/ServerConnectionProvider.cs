using System;
using Afrodite.Abstract;
using Afrodite.Connection.Abstract;
using Apache.NMS;

namespace Afrodite.Connection
{
    public class ServerConnectionProvider<T> : IDisposable
    {
        private readonly IMessageProducer producer;
        private readonly IMessageConsumer consument;
        private readonly ISession session;
        private readonly IConnection connection;
        private readonly IMachinesManager machineManager;
        private readonly TimeSpan recieveTimeout;
        private readonly IStatesManager<T> machineStates;

        public ServerConnectionProvider(NMSConnectionFactory factory, string masterQueueName, long recieveTimeout,
            IMachinesManager machineManager, IStatesManager<T> machineStates)
        {
            this.recieveTimeout = new TimeSpan(recieveTimeout);
            this.machineStates = machineStates;
            this.machineManager = machineManager;
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
            machineManager.Add(props);
        }

        private void ProcessStatusMsg(IMessage message)
        {
            var state = message.ToObject<IComponentState<T>>();
            machineStates[state.MachineId].Add(state);
        }

        public bool StartNewJob<T>(IJob<T> job, int machineId)
        {
            return SendJobActionMessage(job, machineId, MessageType.StartNewJob);
        }

        public bool StopJob(Guid jobId, int machineId)
        {
            return SendJobActionMessage(jobId, machineId, MessageType.StopJob);
        }

        public bool PauseJob(Guid jobId, int machineId)
        {
            return SendJobActionMessage(jobId, machineId,MessageType.ResumeJob);
        }

        public bool ResumeJob(Guid jobId, int machineId)
        {
            return SendJobActionMessage(jobId, machineId, MessageType.ResumeJob);
        }

        private bool SendJobActionMessage<T>(T msgBody, int machineId, MessageType type)
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
                        throw new TimeoutException("Master not answering");
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