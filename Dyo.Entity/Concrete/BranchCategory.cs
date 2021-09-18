using Dyo.Core.DataAccess.MongoDB.Helpers;
using Dyo.Core.Entities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dyo.Entity.Concrete
{
    [BsonCollection("BranchCategories")]
    public class BranchCategory : IEntity
    {
        public ObjectId Id { get; set; }
        public string BranchCategoryName { get; set; }
    }
}
