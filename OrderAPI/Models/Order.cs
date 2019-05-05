using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderAPI.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonElement("UserId")]
        public string UserId { get; set; }

        [BsonElement("Total")]
        public double Total { get; set; }

        [BsonElement("Products")]
        public List<ProductOrder> Products { get; set; }
    }
}
