using Dyo.Core.DataAccess.MongoDB.Helpers;
using Dyo.Core.Entities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dyo.Entity.Concrete
{

    public class Category
    {
        public string CategoryName { get; set; }
        public ETypesOfEducation TypeOfEducation { get; set; }
        public int Branch { get; set; }
    }
}
