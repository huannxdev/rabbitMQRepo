//using Newtonsoft.Json;
//using ProductAPI.Models;
//using ProductAPI.Services;
//using RabbitMQ.Client;
//using RabbitMQ.Client.Events;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ProductAPI.RabbitMQ
//{
//    public class EventBusRabbitMQ
//    {
//        private readonly IRabbitMQPersistentConnection _persistentConnection;
//        private IModel _consumerChannel;
//        private string _queueName;
//        private ProductService _productService;

//        public EventBusRabbitMQ(IRabbitMQPersistentConnection persistentConnection, string queueName = null, ProductService productService)
//        {
//            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
//            _queueName = queueName;
//            _productService = productService;
//        }

//        public IModel CreateConsumerChannel()
//        {
//            if (!_persistentConnection.IsConnected)
//            {
//                _persistentConnection.TryConnect();
//            }

//            var channel = _persistentConnection.CreateModel();
//            channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

//            var consumer = new EventingBasicConsumer(channel);

//            //consumer.Received += async (model, e) =>
//            //{
//            //    var eventName = e.RoutingKey;
//            //    var message = Encoding.UTF8.GetString(e.Body);
//            //    channel.BasicAck(e.DeliveryTag, multiple: false);
//            //};

//            //Create event when something receive
//            consumer.Received += ReceivedEvent;



//            channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
//            channel.CallbackException += (sender, ea) =>
//            {
//                _consumerChannel.Dispose();
//                _consumerChannel = CreateConsumerChannel();
//            };
//            return channel;
//        }

//        private void ReceivedEvent(object sender, BasicDeliverEventArgs e)
//        {
//            if (e.RoutingKey == "userInsertMsgQ")
//            {
//                var message = Encoding.UTF8.GetString(e.Body);
//                List<UpdateCountRequestModel> userList = JsonConvert.DeserializeObject<List<UpdateCountRequestModel>>(message);

//                //PublishUserSaveFeedback("userInsertMsgQ_feedback", saveFeedback, e.BasicProperties.Headers);
//            }

//            if (e.RoutingKey == "emailSendMsgQ")
//            {
//                //Implementation here
//            }
//        }

//        //public void PublishUserSaveFeedback(string _queueName, UserSaveFeedback publishModel, IDictionary<string, object> headers)
//        //{

//        //    if (!_persistentConnection.IsConnected)
//        //    {
//        //        _persistentConnection.TryConnect();
//        //    }

//        //    using (var channel = _persistentConnection.CreateModel())
//        //    {

//        //        channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
//        //        var message = JsonConvert.SerializeObject(publishModel);
//        //        var body = Encoding.UTF8.GetBytes(message);

//        //        IBasicProperties properties = channel.CreateBasicProperties();
//        //        properties.Persistent = true;
//        //        properties.DeliveryMode = 2;
//        //        properties.Headers = headers;
//        //        // properties.Expiration = "36000000";
//        //        //properties.ContentType = "text/plain";

//        //        channel.ConfirmSelect();
//        //        channel.BasicPublish(exchange: "", routingKey: _queueName, mandatory: true, basicProperties: properties, body: body);
//        //        channel.WaitForConfirmsOrDie();

//        //        channel.BasicAcks += (sender, eventArgs) =>
//        //        {
//        //            Console.WriteLine("Sent RabbitMQ");
//        //            //implement ack handle
//        //        };
//        //        channel.ConfirmSelect();
//        //    }
//        //}

//        public void Dispose()
//        {
//            if (_consumerChannel != null)
//            {
//                _consumerChannel.Dispose();
//            }
//        }
//    }
//}
