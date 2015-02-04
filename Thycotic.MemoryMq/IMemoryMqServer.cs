﻿using System.ServiceModel;

namespace Thycotic.MemoryMq
{
    [ServiceContract(Namespace = "http://www.thycotic.com/services", SessionMode = SessionMode.Required, CallbackContract = typeof(IMemoryMqServerCallback))]
    public interface IMemoryMqServer
    {
        [OperationContract(IsOneWay = true)]
        void BasicPublish(string exchangeName, string routingKey, bool mandatory, bool immediate, byte[] body);

        [OperationContract(IsOneWay = true)]
        void QueueBind(string queueName, string exchangeName, string routingKey);

        [OperationContract(IsOneWay = false)]
        void BasicConsume(string queueName);
        
        [OperationContract(IsOneWay = true)]
        void BasicNack(ulong deliveryTag, bool multiple);

        [OperationContract(IsOneWay = true)]
        void BasicAck(ulong deliveryTag, bool multiple);


        
    }
}