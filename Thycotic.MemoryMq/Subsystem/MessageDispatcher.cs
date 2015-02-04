﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Thycotic.Logging;

namespace Thycotic.MemoryMq.Subsystem
{
    /// <summary>
    /// Message dispatcher
    /// </summary>
    public class MessageDispatcher : IDisposable
    {
        private readonly ExchangeDictionary _exchangeDictionary;
        private readonly BindingDictionary _bindings;
        private readonly ClientDictionary _clientDictionary;
        private CancellationTokenSource _cts;
        private Task _monitoringTask;

        private readonly ILogWriter _log = Log.Get(typeof(MessageDispatcher));

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageDispatcher"/> class.
        /// </summary>
        /// <param name="exchangeDictionary">The exchange dictionary.</param>
        /// <param name="bindings">The bindings.</param>
        /// <param name="clientDictionary">The client dictionary.</param>
        public MessageDispatcher(ExchangeDictionary exchangeDictionary, BindingDictionary bindings, ClientDictionary clientDictionary)
        {
            _exchangeDictionary = exchangeDictionary;
            _bindings = bindings;
            _clientDictionary = clientDictionary;

        }

        private void MonitorAndDispatch()
        {
            do
            {
                _exchangeDictionary.Mailboxes.ToList().ForEach(mailbox =>
                {
                    string queueName;
                    if (!_bindings.TryGetBinding(mailbox.RoutingSlip, out queueName))
                    {
                        //no binding for the routing slip
                        return;
                    }

                    MemoryMqServerClientProxy clientProxy;
                    if (!_clientDictionary.TryGetClient(queueName, out clientProxy))
                    {
                        //no client for the queue
                        return;
                    }


                    byte[] body;
                    if (!mailbox.Queue.TryDequeue(out body))
                    {
                        //nothing in the queue
                        return;
                    }

                    clientProxy.Callback.SendMessage(new MemoryQueueDeliveryEventArgs(Guid.NewGuid().ToString(), 1, false, mailbox.RoutingSlip.Exchange,
                        mailbox.RoutingSlip.RoutingKey, new MemoryMqProperties(), body));
                });

            } while (!_cts.IsCancellationRequested);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            Stop();

            _log.Debug("Staring message monitoring");

            _cts = new CancellationTokenSource();
            _monitoringTask = Task.Factory.StartNew(MonitorAndDispatch);
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            if ((_cts == null) || (_monitoringTask == null))
            {
                return;
            }

            _log.Debug("Stopping message monitoring");

            _cts.Cancel();

            _monitoringTask.Wait();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Stop();
        }
    }
}
