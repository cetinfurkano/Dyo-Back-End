using Dyo.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dyo.Entity.DTOs
{
    public class OrderForCreateDto
    {
        public string TeacherId { get; set; }
        public string DistributorId { get; set; }
        public List<OrderItemForCreateDto> Items { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public int OrderState { get; set; }
        public Address Address { get; set; }
        public bool IsValid { get; set; } = true;
    }

    public class OrderItemForCreateDto
    {
        public string ProductId { get; set; }
        public int Count { get; set; }
    }
}
