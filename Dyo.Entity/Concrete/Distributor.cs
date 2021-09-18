using Dyo.Core.DataAccess.MongoDB.Helpers;
using Dyo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace Dyo.Entity.Concrete
{
    [BsonCollection("Distributors")]

    public class Distributor : IEntity
    {
        public Distributor()
        {
            
            Products = new List<ObjectId>();
            Contracts = new List<Contract>();
            Roles = new List<string>();
        }

        public ObjectId Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string OfficeAddress { get; set; }
        public List<string> Roles { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public Rating Rating { get; set; }
        public ProfilePhoto ProfilePhoto { get; set; }
        public List<ObjectId> Products { get; set; }
        public List<ObjectId> Teachers { get; set; }
        public List<Contract> Contracts { get; set; }

    }
}
