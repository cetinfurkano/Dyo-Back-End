using System;
using System.Collections.Generic;
using System.Text;

namespace Dyo.Entity.DTOs
{
    public class ProductForCreateDto
    {
        public string DistributorId { get; set; }
        public string PublisherName { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public CategoryDto ProductCategory { get; set; }
        public double Price { get; set; }
        public double Cost { get; set; }
        public double Discount { get; set; }
        public int StockAmount { get; set; }
        public bool IsValid { get; set; }
    }

    public class CategoryDto
    {
        public string CategoryName { get; set; }
        public int TypeOfEducation { get; set; }
        public int Branch { get; set; }
    }
}
