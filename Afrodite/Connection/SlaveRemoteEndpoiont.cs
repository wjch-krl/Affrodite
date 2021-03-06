﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Afrodite.Abstract;
using Afrodite.Common;
using Afrodite.Concrete;
using Afrodite.Connection.Abstract;
using Apache.NMS;

namespace Afrodite.Connection
{
    public class SlaveRemoteEndpoiont<TJob> : IDisposable
    {
        private IMessageProducer producer;
        private IMessageConsumer consument;
        private ISession session;
        private IConnection connection;
        private IComponentPropertiesBuilder propertiesBuilder;
        private ICurrentMachineStateManager<TJob> componentStateManager;
        private bool run;
        private int stateInnterval;
        private IComponent<TJob> localComponent;
 
        public SlaveRemoteEndpoiont(NMSConnectionFactory factory, int timeout, string
            masterQueueName, string clientQueueName, IComponentPropertiesBuilder propertiesBuilder,
            ICurrentMachineStateManager<TJob> componentStateManager, int stateInnterval)
        {
            this.stateInnterval = stateInnterval;
            this.componentStateManager = componentStateManager;
            this.propertiesBuilder = propertiesBuilder;
			this.localComponent = new LocalComponent<TJob>(Configurator<TJob>.LocalHost.MachineId,Configurator<TJob>.LocalHost.MachineNumber);

            connection = factory.CreateConnection();
            session = connection.CreateSession();
            var masterQueue = session.GetDestination(masterQueueName);

            ConnectToMaster(masterQueue, timeout);

            consument = session.CreateConsumer(session.GetQueue(clientQueueName));
            consument.Listener += ConsumentListener;

            connection.Start();
            run = true;
            Task.Factory.StartNew(StartSendingState);
        }

        private void StartSendingState()
        {
            do
            {
                SendState();
                Thread.Sleep(stateInnterval);
            } while (run);
        }

        private void SendState()
        {
            using (var clientProd = session.CreateProducer())
            {
                var state = componentStateManager.CurrentState();
                var msg = session.CreateObjectMessage(state);
                msg.NMSType = MessageType.Status.ToString();
                clientProd.Send(msg);
            }
        }

        private void ConnectToMaster(IDestination masterQueue, int timeout)
        {
            producer = session.CreateProducer();
            var props = propertiesBuilder.CreateProperties();
            using (var clientProd = session.CreateProducer())
            {
                using (var clientCons = session.CreateConsumer(masterQueue))
                {
                    var msg = session.CreateObjectMessage(props);
                    msg.NMSType = MessageType.ConnectionRequest.ToString();
                    clientProd.Send(msg);
                    var ack = clientCons.Receive(TimeSpan.FromMilliseconds(timeout));
                    if (ack == null)
                    {
                        throw new TimeoutException("Master not answering");
                    }
                    if (!msg.IsAcknowledge())
                    {
                        throw new InvalidOperationException("Master didn't ACK");
                    }
                }
            }
        }

        private void ConsumentListener(IMessage message)
        {
            MessageType type = (MessageType) Enum.Parse(
                typeof (MessageType), message.NMSType, true);
            switch (type)
            {
                case MessageType.PauseJob:
                    ProcessPause(message);
                    AckownlageMessage(message);
                    break;
                case MessageType.ResumeJob:
                    ProcessResume(message);
                    AckownlageMessage(message);
                    break;
                case MessageType.StartNewJob:
                    ProcessStart(message);
                    AckownlageMessage(message);
                    break;
                case MessageType.StopJob:
                    ProcessStop(message);
                    AckownlageMessage(message);
                    break;
            }
        }

        private void ProcessPause(IMessage message)
        {
            throw new NotImplementedException();
        }

        private void ProcessResume(IMessage message)
        {
            throw new NotImplementedException();
        }

        private void ProcessStart(IMessage message)
        {
            TJob job = (TJob)message.ToObject();
            localComponent.StartJob(job);
        }

        private void ProcessStop(IMessage message)
        {
            throw new NotImplementedException();
        }

        private void AckownlageMessage(IMessage message)
        {
            var ack = producer.CreateMessage();
            ack.NMSType = MessageType.Affirmation.ToString();
            producer.Send(message.NMSReplyTo, ack);
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