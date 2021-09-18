using Dyo.Core.DataAccess.MongoDB.Helpers;
using Dyo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace Dyo.Entity.Concrete
{
    [BsonCollection("Products")]
   public class Product: IEntity
    {
        public Product()
        {
            Images = new List<ProductImage>();
            Rating = new Rating();
        }

        public ObjectId Id { get; set; }
        public ObjectId DistributorId { get; set; }
        public string ProductName { get; set; }
        public string PublisherName { get; set; }
        public Rating Rating { get; set; }
        public List<ProductImage> Images { get; set; }     
        public string ProductDescription { get; set; }
        public Category ProductCategory { get; set; }
        public double Price { get; set; }
        public double Cost { get; set; }
        public int StockAmount { get; set; }
        public int Discount { get; set; }
        public bool IsValid { get; set; }

    }
}
