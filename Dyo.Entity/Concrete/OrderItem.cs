using Dyo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dyo.Entity.Concrete
{
    public class OrderItem
    {
        public Product Product { get; set; }
        public int Count { get; set; }
        
    }
}
