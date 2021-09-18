using System;
using System.Collections.Generic;
using System.Text;

namespace Dyo.Entity.DTOs
{
    public class ContractForCreateDto
    {
        public string DistributorId { get; set; }
        public string PublisherName { get; set; }
        public double DealAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public double MaxDiscount { get; set; }
        public bool IsValid { get; set; }
    }
}
