using Dyo.Core.DataAccess.MongoDB.Helpers;
using Dyo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dyo.Entity.Concrete
{
   
    public class Rating
    {
        public int VoteCount { get; set; }
        public int TotalPoint { get; set; }
        public double Avarage { get; set; }
    }
}
