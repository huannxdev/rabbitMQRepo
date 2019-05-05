using Newtonsoft.Json;
using OrderAPI.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.RabbitMQ
{
    public class RabbitMQClient
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _model;

        private const string ExchangeName = "Topic_Exchange";
        private const string UpdateProductQueueName = "UpdateProducTopic_Queue";

        public RabbitMQClient()
        {
            CreateConnection();
        }

        private static void CreateConnection()
        {
            _factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = _factory.CreateConnection();
            _model = _connection.CreateModel();
            _model.ExchangeDeclare(ExchangeName, "topic");

            _model.QueueDeclare(UpdateProductQueueName, true, false, false, null);
            _model.QueueBind(UpdateProductQueueName, ExchangeName, "product.updateProduct");
        }

        public void Close()
        {
            _connection.Close();
        }

        public void SendUpdateProductRequest(List<ProductOrder> productOrders)
        {
            var json = JsonConvert.SerializeObject(productOrders);
            var message = Encoding.ASCII.GetBytes(json);
            SendMessage(message, "product.updateProduct");
        }

        public void SendMessage(byte[] message, string routingKey)
        {
            _model.BasicPublish(ExchangeName, routingKey, null, message);
        }
    }
}