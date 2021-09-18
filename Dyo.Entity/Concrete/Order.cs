using Dyo.Core.DataAccess.MongoDB.Helpers;
using Dyo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace Dyo.Entity.Concrete
{
    [BsonCollection("Orders")]

    public class Order: IEntity
    {
        public Order()
        {
            Items = new List<OrderItem>();
        }
        public ObjectId Id { get; set; }
        public Teacher Teacher { get; set; }
        public Distributor Distributor { get; set; }
        public List<OrderItem> Items { get; set; }
        public EOrderState OrderState { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsValid { get; set; }
        public Address Address { get; set; }

    }
}
