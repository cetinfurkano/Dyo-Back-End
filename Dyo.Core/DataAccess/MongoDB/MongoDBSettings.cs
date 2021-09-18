using System;
using System.Collections.Generic;
using System.Text;

namespace Dyo.Core.DataAccess.MongoDB
{
    public class MongoDBSettings : IMongoDBSettings
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
    }
}
