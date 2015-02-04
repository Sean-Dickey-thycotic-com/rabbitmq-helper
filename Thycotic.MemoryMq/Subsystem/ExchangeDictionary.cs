using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Thycotic.Logging;

namespace Thycotic.MemoryMq.Subsystem
{
    /// <summary>
    /// Exchange that binds topics to queues
    /// </summary>
    public class ExchangeDictionary 
    {
        private readonly ConcurrentDictionary<RoutingSlip, ConcurrentQueue<MemoryQueueDeliveryEventArgs>> _data = new ConcurrentDictionary<RoutingSlip, ConcurrentQueue<MemoryQueueDeliveryEventArgs>>();

        private readonly ILogWriter _log = Log.Get(typeof(ExchangeDictionary));

        /// <summary>
        /// Gets the mailboxes in the exchange
        /// </summary>
        /// <value>
        /// The mailboxes.
        /// </value>
        public IList<Mailbox> Mailboxes 
        {
            get { return _data.Select(kvp => new Mailbox(kvp.Key, kvp.Value)).ToList(); }
        }

        /// <summary>
        /// Publishes the body to the specified routing slip.
        /// </summary>
        /// <param name="routingSlip">The routing slip.</param>
        /// <param name="body">The body.</param>
        public void Publish(RoutingSlip routingSlip, MemoryQueueDeliveryEventArgs body)
        {
            _log.Debug(string.Format("Publishing message to {0}", routingSlip));

            _data.GetOrAdd(routingSlip, s => new ConcurrentQueue<MemoryQueueDeliveryEventArgs>());

            _data[routingSlip].Enqueue(body);
        }
    }
}