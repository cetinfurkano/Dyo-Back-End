using Dyo.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dyo.Entity.DTOs
{
    public class OrderForResultDto
    {
        public string Id { get; set; }
        public DistributorForOrderResultDto Distributor { get; set; }
        public TeacherForOrderResult Teacher { get; set; }
        public List<OrderItemForResultDto> Items { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public int OrderState { get; set; }
        public Address Address { get; set; }
        public bool IsValid { get; set; } = true;
    }

    public class OrderItemForResultDto
    {
        public ProductForResultDto Product { get; set; }
        public int Count { get; set; }
    }
}
