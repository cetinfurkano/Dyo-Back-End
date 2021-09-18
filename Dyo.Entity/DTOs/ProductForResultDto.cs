using Dyo.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dyo.Entity.DTOs
{
    public class ProductForResultDto
    {
        public string Id { get; set; }
        public string DistributorId { get; set; }
        public string PublisherName { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double Price { get; set; }
        public double Cost { get; set; }
        public int StockAmount { get; set; }
        public int Discount { get; set; }
        public bool IsValid { get; set; }
        public CategoryDto ProductCategory { get; set; }
        public List<ProductImage> Images { get; set; } = new List<ProductImage>();
        public Rating Rating { get; set; }

    }
}
