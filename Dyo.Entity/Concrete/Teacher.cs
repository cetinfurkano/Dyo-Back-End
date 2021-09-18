using Dyo.Core.DataAccess.MongoDB.Helpers;
using Dyo.Core.Entities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dyo.Entity.Concrete
{
    [BsonCollection("Teachers")]
    public class Teacher : IEntity
    {
        public Teacher()
        {
            Roles = new List<string>();
            Distributors = new List<ObjectId>();
        }

        public ObjectId Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string School { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt{ get; set; }
        public List<string> Roles { get; set; }
        public List<ObjectId> Distributors { get; set; }
        public DateTime LastUpdated { get; set; }
        public Address Address { get; set; }
        public string Branch { get; set; }

    }
}
