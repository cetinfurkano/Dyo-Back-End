using System;
using System.Collections.Generic;
using System.Text;

namespace Dyo.Business.Helpers
{
    public class DistributorStatistics
    {
        public int LinkedTeacherCount { get; set; }
        public int TotalOrder { get; set; }
        public int ProductCount { get; set; }
        public double TurnoverThisMonth { get; set; }
        public int TotalOrderThisMonth { get; set; }
        public double TotalGainThisMonth { get; set; }
        public int PublishersCount { get; set; }
        public double TotalCostThisMonth { get; set; }
    }
}
