using Dyo.Core.DataAccess.MongoDB.Helpers;
using Dyo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace Dyo.Entity.Concrete
{
    [BsonCollection("Comments")]

    public class Comment: IEntity
    {
        public ObjectId Id { get; set; }
        public Teacher Teacher { get; set; }
        public Product CommentedProduct { get; set; }
        public DateTime CommentDate { get; set; }
        public double Pleasure { get; set; }
        public string CommentText { get; set; }
    }
}
