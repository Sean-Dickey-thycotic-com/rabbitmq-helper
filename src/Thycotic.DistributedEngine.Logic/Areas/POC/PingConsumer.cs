﻿using System;
using System.Linq;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Logging;
using Thycotic.Messages.Areas.POC.Request;
using Thycotic.Messages.Common;

namespace Thycotic.DistributedEngine.Logic.Areas.POC
{
    /// <summary>
    /// Simple Hello World consumer
    /// </summary>
    public class PingConsumer : IBasicConsumer<PingMessage>
    {
        private readonly IResponseBus _responseBus;

        private readonly ILogWriter _log = Log.Get(typeof(PingConsumer));

        /// <summary>
        /// Initializes a new instance of the <see cref="PingConsumer" /> class.
        /// </summary>
        /// <param name="responseBus">The response bus.</param>
        public PingConsumer(IResponseBus responseBus)
        {
            _responseBus = responseBus;
        }


        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Consume(PingMessage request)
        {
            _log.Debug(string.Format("Consuming ping sequence #{0}", request.Sequence));

            try
            {
                _responseBus.ExecuteAsync(new EnginePingRequest());
            }
            catch (Exception ex)
            {
                _log.Error("Failed to pong back to server", ex);
            }
        }
    }
}
