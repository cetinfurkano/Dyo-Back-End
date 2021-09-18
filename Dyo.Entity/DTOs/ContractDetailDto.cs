using System;
using System.Collections.Generic;
using System.Text;

namespace Dyo.Entity.DTOs
{
    public class ContractDetailDto
    {
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public double DealAmount { get; set; }
        public double MaxDiscount { get; set; }
        public double Revenues { get; set; }
        public int CurrentPercent { get; set; }
        public double RequiredAmount { get; set; }
    }
}
