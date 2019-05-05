using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Newtonsoft.Json;
using ProductAPI.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductAPI.Services
{
    public class ProductService
    {
        private readonly IMongoCollection<Product> _products;

        public ProductService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("eshopStore"));
            var database = client.GetDatabase("eshopStore");
            _products = database.GetCollection<Product>("Products");
            this.updateTotalQuantityhandler();
        }

        public List<Product> Get()
        {
            return _products.Find(product => true).ToList();
        }

        public Product Get(string id)
        {
            return _products.Find<Product>(product => product.Id == id).FirstOrDefault();
        }

        public Product Create(Product product)
        {
            _products.InsertOne(product);
            return product;
        }

        public void Update(string id, Product productIn)
        {
            _products.ReplaceOne(product => product.Id == id, productIn);
        }

        public void Remove(Product productIn)
        {
            _products.DeleteOne(product => product.Id == productIn.Id);
        }

        public void Remove(string id)
        {
            _products.DeleteOne(product => product.Id == id);
        }


        public bool UpdateCount(string id, int quantity)
        {
            var result = _products.Find<Product>(product => product.Id == id).FirstOrDefault();
            if(result != null)
            {
                result.Count -= quantity;
                _products.ReplaceOne(product => product.Id == id, result);
                return true;
            }
            return false;
        }

        private void updateTotalQuantityhandler()
        {
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "UpdateProducTopic_Queue", //UpdateProducTopic_Queue
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                // cai nay la consumer, thang sender dau ?
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    UpdateCountRequestModel[] requests = JsonConvert.DeserializeObject<UpdateCountRequestModel[]>(message);
                    if(requests != null)
                    {
                        for(int i = 0; i < requests.Length; i++)
                        {
                            this.UpdateCount(requests[i].Id, requests[i].Quantity);
                        }
                    }
                };
                channel.BasicConsume(queue: "UpdateProducTopic_Queue",
                                     autoAck: true,
                                     consumer: consumer);
            }
        }
    }
}
