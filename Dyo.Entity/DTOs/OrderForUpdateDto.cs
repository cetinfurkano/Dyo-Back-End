using Dyo.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dyo.Entity.DTOs
{
    public class OrderForUpdateDto
    {
        public string OrderId { get; set; }
        public string TeacherId { get; set; }
        public string DistributorId { get; set; }
        public List<OrderItemForCreateDto> OrderItems { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public int OrderState { get; set; }
        public Address Address { get; set; }
        public bool IsValid { get; set; } = true;
    }
}
