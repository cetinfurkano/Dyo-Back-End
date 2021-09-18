using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dyo.Core.Entities
{
    public interface IEntity
    {
        ObjectId Id { get; set; }
    }
}
