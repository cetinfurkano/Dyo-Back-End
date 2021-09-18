using Dyo.Core.DataAccess.MongoDB.Helpers;
using Dyo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace Dyo.Entity.Concrete
{
    public class Contract
    {
        public string PublisherName { get; set; }
        public double DealAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public double MaxDiscount { get; set; }
        public bool IsValid { get; set; }
        public double Revenues { get; set; }
        public int CurrentPercent { get; set; }
        public double RequiredAmount { get; set; }

    }
}
