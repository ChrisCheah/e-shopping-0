using e_orderservices.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Core.WireProtocol.Messages.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_shoporderservices.Models
{
    public class DataAccess
    {
        MongoClient _client;
        //MongoServer _server;
        IMongoDatabase _db;
        private readonly IMongoCollection<Order> _orders;


        public DataAccess(IOrderSettings orderSettings)
        {
            _client = new MongoClient(orderSettings.ConnectionString);
            _db = _client.GetDatabase(orderSettings.DatabaseName);
            _orders = _db.GetCollection<Order>(orderSettings.OrdersCollectionName);
            
        }

        public IEnumerable<Order> GetOrders()
        {
            List<Order> orders;
            orders = _orders.Find(emp => true).ToList();
            return orders;
        }


        public Order GetOrder(ObjectId id)
        {
            var res = Query<Order>.EQ(p => p.Id, id);
            return _orders.Find<Order>(order => order.Id== id).FirstOrDefault();
        }

        public Order Create(Order order)
        {
            _orders.InsertOne(order);
            
            return order;
        }

    }
}
